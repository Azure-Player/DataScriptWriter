using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CAMOsoft.DbUtils
{

    public class MsSqlSession : DbSession
    {

        #region *** members ***

        private int m_SPID = 0;        
        //private MsSqlCmd m_CurrentSP = null;

        #endregion

        #region *** SYSTEM ***

        public MsSqlSession(string pConnectionString) : base(pConnectionString) { }

        public MsSqlSession(bool UsePoolingConnection) : base(UsePoolingConnection) { }

        public void LoginWindowsAuth(string pDbServer, string pDbName)
        {
            base.Login(pDbServer, null, null, pDbName);
        }
        public void LoginWindowsAuth(string pDbServer)
        {
            base.Login(pDbServer, null, null, null);
        }

        public void LoginSQLAuth(string pDbServer, string pDbUserName, string pDbUserPass, string pDbName)
        {
            base.Login(pDbServer, pDbUserName, pDbUserPass, pDbName);
        }
        public void LoginSQLAuth(string pDbServer, string pDbUserName, string pDbUserPass)
        {
            base.Login(pDbServer, pDbUserName, pDbUserPass, null);
        }

        public void ChangeDatabase(string dbName)
        {
            base.Execute("USE " + QuotedName(dbName));
        }

        protected override void PostInit()
        {
            //m_CurrentSP = new MsSqlCmd(this);
        }

        #endregion

        #region *** ABSTRACT ***

        public override DbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }
        public override DbCommand CreateCommand()
        {
            return new SqlCommand();
        }
        public override DbCommandBuilder CreateCommandBuilder(DbDataAdapter pDa)
        {
            return new SqlCommandBuilder(pDa as SqlDataAdapter);
        }
        public override string GetConnectionString()
        {
            SqlConnectionStringBuilder MyBuilder = new SqlConnectionStringBuilder();
            MyBuilder.DataSource = m_DbServer;
            MyBuilder.InitialCatalog = m_DbName == null ? "" : m_DbName;
            if (m_DbUserName == null && m_DbUserPass == null)
                MyBuilder.IntegratedSecurity = true;
            else
            {
                MyBuilder.UserID = m_DbUserName;
                MyBuilder.Password = m_DbUserPass;
            }
            MyBuilder.Pooling = m_Pooling;
            if (m_Pooling)
                MyBuilder.MaxPoolSize = m_MaxPoolingSize;
            else
                MyBuilder.MaxPoolSize = 1;
            return MyBuilder.ConnectionString;
        }
        public override DbConnection CreateConnection(string pConnectionString)
        {
            return new SqlConnection(pConnectionString);
        }

        #endregion

        #region *** UPDATE / SAVE ***

        protected override void PrepareInsert(DbDataAdapter pDA, DataTable pDt)
        {
            DbCommandBuilder MyCB = CreateCommandBuilder(pDA);
            DbCommand MyCmdSrc = MyCB.GetInsertCommand();
            DbCommand MyCmdDest = CreateCommand(MyCmdSrc.CommandText);
            
            if (m_PrimaryKeyRequest && (pDt.PrimaryKey == null || pDt.PrimaryKey.Length == 0))
                throw new Exception("PrimaryKey isn't exist!");

            if (m_PrimaryKeyRequest && pDt.PrimaryKey[0].AutoIncrement)
            {
                MyCmdDest.CommandText = MyCmdDest.CommandText + "; SELECT SCOPE_IDENTITY() AS " + pDt.PrimaryKey[0].ColumnName;
                MyCmdDest.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;
            }
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
            pDA.InsertCommand.Transaction = base.Transaction;
        }

        public void BulkInsert(DataTable pTable, string pDstTableName)
        {
            System.Data.SqlClient.SqlBulkCopy mySqlBulk = new SqlBulkCopy(this.Connection as SqlConnection, SqlBulkCopyOptions.Default, this.Transaction as SqlTransaction);
            mySqlBulk.DestinationTableName = "tmpBulkInsert";
            mySqlBulk.WriteToServer(pTable);
        }

        #endregion

        #region *** PUBLIC METHODS *** 

        public new string Quote(string pSqlText)
        {
            return pSqlText.Replace("'", "''");
        }

        public override string QuotedName(string pName)
        {
            return pName.StartsWith("[") ? pName : "[" + pName + "]";
        }

        #endregion

        #region *** PUBLIC  SELECT  ***


        public int SelectValueIntParam(string pSql, params object[] pParamValues)
        {
            return int.Parse(SelectValueParam(pSql, pParamValues).ToString());
        }

        public object SelectValueParam(string pSql, params object[] pParamValues)
        {
            if (pParamValues.Length == 0) return base.SelectValue(pSql);

            MsSqlCmd myCmd = new MsSqlCmd(this);
            myCmd.SetSql(pSql);
            myCmd.AddParamsIn(pParamValues);
            return myCmd.ExecuteScalar();
        }


        #endregion

        #region *** PROPERTIES ***

        public int SPID
        {
            get 
            {
                if (m_SPID == 0 && base.IsLogged)
                {
                    m_SPID = SelectValueInt("SELECT @@SPID");
                }
                return m_SPID;
            }
        }

        #endregion

    }

}
