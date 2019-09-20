using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Data.SqlClient;

namespace CAMOsoft.DbUtils
{
    public abstract class DbSession
    {

        #region *** members ***

        private bool m_Logged;
        protected bool m_PrimaryKeyRequest = true;
        protected int m_CommandTimeoutDef = 90;
        protected int m_CommandTimeout = 90;
        protected string m_DbServer;
        protected string m_DbName;
        protected string m_DbUserName;
        protected string m_DbUserPass;
        protected bool m_Pooling = true;
        protected int m_MaxPoolingSize = 100;
        public Exception LoginException = null;

        private DbConnection m_Connection;
        private DbTransaction m_Trans;
        private bool m_MultiTransactionMode = false;
        private int m_TransactionCount = 0;

        private int m_BatchSize = 1;
        private string m_CurrentUpdating = "";

        #endregion

        #region *** SYSTEM ***

        public DbSession()
        {
        }

        public DbSession(bool pooling)
        {
            m_Pooling = pooling;
        }

        public DbSession(string pConnectionString)
        {
            Connect(pConnectionString);
            PostInit();
        }

        protected void Login(string pDbServer, string pDbUserName, string pDbUserPass, string pDbName)
        {
            m_DbServer = pDbServer;
            m_DbName = pDbName;
            m_DbUserName = pDbUserName;
            m_DbUserPass = pDbUserPass;
            Connect();
            PostInit();
        }

        protected void Login(string pDbServer, string pDbUserName, string pDbUserPass, string pDbName, bool pPooling)
        {
            m_DbServer = pDbServer;
            m_DbName = pDbName;
            m_DbUserName = pDbUserName;
            m_DbUserPass = pDbUserPass;
            m_Pooling = pPooling;
            Connect();
            PostInit();
        }

        protected virtual void PostInit()
        {

        }

        #endregion

        #region *** ABSTRACT ***

        public abstract DbDataAdapter CreateDataAdapter();
        public abstract DbCommand CreateCommand();
        public abstract DbCommandBuilder CreateCommandBuilder(DbDataAdapter pDa);
        public abstract string GetConnectionString();
        public abstract DbConnection CreateConnection(string pConnectionString);

        #endregion

        #region *** SYSTEM PUBLIC - BEGIN/END ***

        public void CloseConnection()
        {
            if (m_Connection.State == ConnectionState.Open)
                m_Connection.Close();
        }

        #endregion

        #region *** PUBLIC  SELECT  ***

        public DataRow SelectRow(String pSql)
        {
            DataTable MyTable = SelectTable(pSql, "Table0");
            if (MyTable.Rows.Count > 0)
                return MyTable.Rows[0];
            else
                return null;
        }

        public object SelectValue(string pSql)
        {
            DbCommand MyCommand;
            MyCommand = CreateCommand(pSql);
            return MyCommand.ExecuteScalar();
        }

        public Guid SelectValueGuid(string pSql)
        {
            return new Guid(SelectValue(pSql).ToString());
        }

        public int SelectValueInt(string pSql)
        {
            return int.Parse(SelectValue(pSql).ToString());
        }

        public long SelectValueLong(string pSql)
        {
            return long.Parse(SelectValue(pSql).ToString());
        }

        public DbDataAdapter CreateDataAdapter(string pSelectCmdSql)
        {
            DbDataAdapter MyDa = CreateDataAdapter();
            MyDa.SelectCommand = CreateCommand(pSelectCmdSql);
            return MyDa;
        }

        public DbDataAdapter CreateDataAdapter(DataSet pDs, String pTableName)
        {
            DbDataAdapter MyDa = CreateDataAdapter();
            String MyColumns = "";

            foreach (DataColumn MyCol in pDs.Tables[pTableName].Columns)
                MyColumns += "," + QuotedName(MyCol.ColumnName);
            if (MyColumns.Length > 0) MyColumns = MyColumns.Remove(0, 1);
            MyDa.SelectCommand = CreateCommand("SELECT " + MyColumns + " FROM " + QuotedName(pTableName) + " WHERE 0=1");
            return MyDa;
        }

        protected DataTable SelectData(string pSql, string pName, bool pWithSchema)
        {
            DbDataAdapter MyDA = CreateDataAdapter();
            DataSet MyDs = new DataSet();
            MyDA.SelectCommand = CreateCommand(pSql);
            if (pWithSchema)
                MyDA.FillSchema(MyDs, SchemaType.Source, pName);
            MyDA.Fill(MyDs, pName);
            DataTable MyTable = MyDs.Tables[0];
            MyDs.Tables.Remove(MyTable);
            return MyTable;
        }

        public DataTable SelectTable(string pSql, string pTableName)
        {
            return SelectData(pSql, pTableName, true);
        }

        public DataTable SelectView(string pSql, string pViewName)
        {
            return SelectData(pSql, pViewName, false);
        }

        public void SelectDsData(DataSet pDs, string pSql, string pTableName, bool pWithSchema)
        {
            DbDataAdapter MyDA = CreateDataAdapter();
            MyDA.SelectCommand = CreateCommand(pSql);
            if (pWithSchema)
                MyDA.FillSchema(pDs, SchemaType.Source, pTableName);
            MyDA.Fill(pDs, pTableName);
        }

        public void SelectDs(DataSet pDs, string pSql, string pTableName)
        {
            SelectDsData(pDs, pSql, pTableName, true);
        }

        public void SelectDsData(DataSet pDs, string pSql, string pTableName, bool pWithSchema, int pStartRecord, int pMaxRecord)
        {
            DbDataAdapter MyDA = CreateDataAdapter();
            MyDA.SelectCommand = CreateCommand(pSql);
            if (pWithSchema)
                MyDA.FillSchema(pDs, SchemaType.Source, pTableName);
            MyDA.Fill(pDs, pStartRecord, pMaxRecord, pTableName);
        }

        public void SelectDs(DataSet pDs, string pSql, string pTableName, int pStartRecord, int pMaxRecord)
        {
            SelectDsData(pDs, pSql, pTableName, true, pStartRecord, pMaxRecord);
        }

        public void SelectDsView(DataSet pDs, string pSql, string pViewName)
        {
            DbDataAdapter MyDA = CreateDataAdapter();
            MyDA.SelectCommand = CreateCommand(pSql);
            MyDA.Fill(pDs, pViewName);
        }

        public void SelectDsView(DataSet pDs, string pSql, string pViewName, int pStartRecord, int pMaxRecord)
        {
            DbDataAdapter MyDA = CreateDataAdapter();
            MyDA.SelectCommand = CreateCommand(pSql);
            MyDA.Fill(pDs, pStartRecord, pMaxRecord, pViewName);
        }

        public void SelectSchema(DataSet pDs, string pTableName)
        {
            SelectSchema(pDs, "SELECT * FROM " + QuotedName(pTableName) + " WHERE 0 = 1", pTableName);
        }

        public void SelectSchema(DataSet pDs, string pSql, String pTableName)
        {
            DbDataAdapter MyDa = CreateDataAdapter();
            MyDa.SelectCommand = CreateCommand(pSql);
            MyDa.FillSchema(pDs, SchemaType.Source, pTableName);
        }

        #endregion

        #region *** COMMANDS ***

        public DbCommand CreateCommand(string pSql)
        {
            DbCommand MyCmd = CreateCommand();
            MyCmd.CommandText = pSql;
            MyCmd.Connection = m_Connection;
            MyCmd.Transaction = m_Trans;
            MyCmd.CommandTimeout = m_CommandTimeout;
            return MyCmd;
        }

        public int Execute(string pSql)
        {
            DbCommand MyCommand = CreateCommand(pSql);
            return MyCommand.ExecuteNonQuery();
        }

        public int ExecuteNoTimeout(string pSql)
        {
            DbCommand MyCommand = CreateCommand(pSql);
            MyCommand.CommandTimeout = 0;
            return MyCommand.ExecuteNonQuery();
        }

        #endregion

        #region *** UPDATE / SAVE ***

        public void Update(DataSet pDs, string pTableName)
        {
            Update(pDs, pTableName, 1);
        }

        public void Update(DataSet pDs, string pTableName, int pBatchSize)
        {
            if (pDs.Tables[pTableName] == null)
                throw new Exception("This table don't exist!");
            else
            {
                List<string> MyTableOrder = new List<string>();
                MyTableOrder.Add(pTableName);
                Update(pDs, MyTableOrder, pBatchSize);
            }
        }

        public void Update(DataSet pDs, string[] pTableOrder)
        {
            List<string> MyTableOrder = new List<string>(pTableOrder);
            Update(pDs, MyTableOrder, 1);
        }

        private void Update(DataSet pDs, List<string> pTableOrder, int pBatchSize)
        {
            m_CurrentUpdating = "";
            m_BatchSize = pBatchSize;
            pTableOrder.Reverse();
            PerformDelete(pDs, pTableOrder);
            pTableOrder.Reverse();
            PerformInsert(pDs, pTableOrder);
            PerformUpdate(pDs, pTableOrder);
            m_CurrentUpdating = "";
        }

        private void PerformInsert(DataSet pDs, List<string> pTableOrder)
        {
            foreach (string MyTableName in pTableOrder)
            {
                DataRow[] MyRows = pDs.Tables[MyTableName].Select("", "", DataViewRowState.Added);
                if (MyRows.Length > 0)
                {
                    m_CurrentUpdating = MyTableName;
                    DbDataAdapter MyDa = CreateDataAdapter(pDs, MyTableName);
                    PrepareInsert(MyDa, pDs.Tables[MyTableName]);
                    MyDa.UpdateBatchSize = m_BatchSize;
                    if (m_BatchSize > 1)
                        MyDa.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                    MyDa.Update(MyRows);
                }
            }
        }

        private void PerformUpdate(DataSet pDs, List<string> pTableOrder)
        {
            foreach (string MyTableName in pTableOrder)
            {
                DataRow[] MyRows = pDs.Tables[MyTableName].Select("", "", DataViewRowState.ModifiedCurrent);
                if (MyRows.Length > 0)
                {
                    m_CurrentUpdating = MyTableName;
                    DbDataAdapter MyDa = CreateDataAdapter(pDs, MyTableName);
                    DbCommandBuilder MyCB = CreateCommandBuilder(MyDa);
                    MyDa.UpdateCommand = MyCB.GetUpdateCommand();
                    MyDa.UpdateCommand.Transaction = m_Trans;
                    MyDa.UpdateBatchSize = m_BatchSize;
                    MyDa.Update(MyRows);
                }
            }
        }

        private void PerformDelete(DataSet pDs, List<string> pTableOrder)
        {
            foreach (string MyTableName in pTableOrder)
            {
                DataRow[] MyRows = pDs.Tables[MyTableName].Select("", "", DataViewRowState.Deleted);
                if (MyRows.Length > 0)
                {
                    m_CurrentUpdating = MyTableName;
                    DbDataAdapter MyDa = CreateDataAdapter(pDs, MyTableName);
                    DbCommandBuilder MyCB = CreateCommandBuilder(MyDa);
                    MyDa.DeleteCommand = MyCB.GetDeleteCommand();
                    MyDa.DeleteCommand.Transaction = m_Trans;
                    MyDa.UpdateBatchSize = m_BatchSize;
                    MyDa.Update(MyRows);
                }
            }
        }

        protected virtual void PrepareInsert(DbDataAdapter pDA, DataTable pDt)
        {
            DbCommandBuilder MyCB = CreateCommandBuilder(pDA);
            DbCommand MyCmdSrc = MyCB.GetInsertCommand();
            DbCommand MyCmdDest = CreateCommand(MyCmdSrc.CommandText);

            if (m_PrimaryKeyRequest && (pDt.PrimaryKey == null || pDt.PrimaryKey.Length == 0))
                throw new Exception("PrimaryKey isn't exist!");

            if (m_PrimaryKeyRequest && pDt.PrimaryKey[0].AutoIncrement)
                MyCmdDest.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;
            else
                MyCmdDest.UpdatedRowSource = UpdateRowSource.None;

            int MyParamCount = MyCmdSrc.Parameters.Count;
            DbParameter[] MyP = new DbParameter[MyParamCount];

            MyCmdSrc.Parameters.CopyTo(MyP, 0);
            while (MyCmdSrc.Parameters.Count > 0)
                MyCmdSrc.Parameters.Remove(MyCmdSrc.Parameters[MyCmdSrc.Parameters.Count - 1]);

            for (int i = 0; i < MyParamCount; i++)
                MyCmdDest.Parameters.Add(MyP[i]);

            pDA.InsertCommand = MyCmdDest;
            pDA.InsertCommand.Transaction = m_Trans;
        }

        #endregion

        #region *** Transaction managment ***

        public bool TransactionBegin()
        {
            if (m_Trans == null || m_MultiTransactionMode)
            {
                if (m_Trans == null)
                {
                    m_Trans = m_Connection.BeginTransaction();
                }
                m_TransactionCount++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TransactionCommit()
        {
            if (m_Trans != null || m_MultiTransactionMode)
            {
                if (m_MultiTransactionMode && m_TransactionCount <= 0)
                {
                    throw new Exception("The COMMIT TRANSACTION request has no corresponding BEGIN TRANSACTION.");
                }
                m_TransactionCount--;
                if (m_TransactionCount == 0)
                {
                    m_Trans.Commit();
                    m_Trans = null;
                }
            }
        }

        public void TransactionRollback()
        {
            if (m_MultiTransactionMode && m_TransactionCount <= 0)
            {
                throw new Exception("The ROLLBACK TRANSACTION request has no corresponding BEGIN TRANSACTION.");
            }
            try
            {
                if (m_Trans != null)
                {
                    m_Trans.Rollback();
                }
            }
            catch (InvalidOperationException)
            {
                Connect();
            }
            m_Trans = null;
            m_TransactionCount = 0;
        }

        public bool IsTransaction()
        {
            return (m_Trans != null);
        }

        #endregion

        #region *** PUBLIC METHODS ***

        public virtual string Quote(string pSqlText)
        {
            return pSqlText.Replace("'", "''");
        }

        public virtual string QuotedName(string pName)
        {
            return pName;
        }


        #endregion

        #region *** PRIVATE Methods ***

        protected void Connect()
        {
            Connect(GetConnectionString());
        }

        protected void Connect(string pConnectionString)
        {
            try
            {
                m_Connection = CreateConnection(pConnectionString);
                m_Connection.Open();
                m_Logged = (m_Connection.State == ConnectionState.Open);
                LoginException = null;
            }
            catch (Exception ex)
            {
                LoginException = ex;
                m_Logged = false;
            }
        }

        #endregion

        #region *** PROPERTIES ***

        public DbConnection Connection
        {
            get { return m_Connection; }
        }

        public string DbName
        {
            get { return m_DbName; }
        }

        public bool IsLogged
        {
            get { return m_Logged; }
        }

        public int DefautTimeOut
        {
            get { return m_CommandTimeoutDef; }
            set { m_CommandTimeoutDef = value; }
        }

        public int TimeOut
        {
            get { return m_CommandTimeout; }
            set { m_CommandTimeout = value; }
        }

        public bool PrimaryKeyRequest
        {
            get { return m_PrimaryKeyRequest; }
            set { m_PrimaryKeyRequest = value; }
        }

        public string ServerVersion
        {
            get { return m_Connection.ServerVersion; }
        }

        public DbTransaction Transaction
        {
            get { return m_Trans; }
        }

        public bool InTransaction
        {
            get { return IsTransaction(); }
        }
        
        public bool IsMultiTransactionMode
        {
            get { return m_MultiTransactionMode; }
            set
            {
                if (IsTransaction())
                {
                    throw new Exception("Disallow set MultiTransaction Mode during transaction open.");
                }
                else
                {
                    m_MultiTransactionMode = value;
                }
            }
        }

        public int TransactionCount
        {
            get { return m_TransactionCount; }
        }

        #endregion

    }

}
