using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CAMOsoft.DbUtils
{
    public class MsSqlCmd
    {
        private MsSqlSession m_DbSession = null;
        private SqlCommand m_Command = null;
        private String m_ReturnParam = "";
        private String m_Name = "";

        public MsSqlCmd(MsSqlSession pDbSession)
        {
            m_DbSession = pDbSession;
        }
        public MsSqlCmd(MsSqlSession pDbSession, SqlCommand pSqlCommand)
        {
            m_DbSession = pDbSession;
            m_Command = pSqlCommand;
        }

        private void Init()
        {
            m_Command = null;
            m_Name = "";
            m_ReturnParam = "";
        }

        public void SetName(string pName)
        {
            Init();
            m_Name = pName;
            m_Command = m_DbSession.CreateCommand(pName) as SqlCommand;
            m_Command.CommandType = CommandType.StoredProcedure;
        }

        public void SetSql(string pSql)
        {
            Init();
            m_Command = m_DbSession.CreateCommand(pSql) as SqlCommand;
            m_Command.CommandType = CommandType.Text;
        }

        public void AddParamIn(String pName, object pValue)
        {
            SqlParameter MyParam;
            if (pValue != null)
                MyParam = new SqlParameter(pName, pValue);
            else
                MyParam = new SqlParameter(pName, DBNull.Value);
            MyParam.Direction = ParameterDirection.Input;
            if (pValue != null && pValue.GetType() == typeof(string))
                MyParam.Size = (int)Math.Ceiling((decimal)pValue.ToString().Length / 16m) * 16;
            m_Command.Parameters.Add(MyParam);
        }

        public void AddParamsIn(object[] pParamValues)
        {
            Asserts.Assert(m_Command != null, "No current SqlCommand!");
            int myIndex = 0;
            Regex myRegEx = new Regex("@[_A-Za-z]+[0-9]*");
            MatchCollection myParamNames = myRegEx.Matches(m_Command.CommandText);
            Asserts.Assert(myParamNames.Count == pParamValues.Length, "Inna liczba parametrów ni¿ ich wartoœci!");
            foreach (object mySqlValue in pParamValues)
            {
                AddParamIn(myParamNames[myIndex].Value, mySqlValue);
                myIndex++;
            }
        }

        public void AddParamOut(String pName, DbType pDbType)
        {
            m_ReturnParam = pName;
            SqlParameter MyParam = new SqlParameter();
            MyParam.ParameterName = pName;
            MyParam.DbType = pDbType;
            if (pDbType == DbType.String) MyParam.Size = 1024;
            MyParam.Direction = ParameterDirection.Output;
            m_Command.Parameters.Add(MyParam);
        }
        
        public int Execute()
        {
            Asserts.Assert(m_Command != null, "No current SqlCommand!");
            return m_Command.ExecuteNonQuery();
        }

        public DataTable ExecuteWithDataTable()
        {
            return ExecuteWithDataTable(false);
        }

        public DataTable ExecuteWithDataTable(bool pFillSchema)
        {
            Asserts.Assert(m_Command != null, "No current SqlCommand!");
            SqlDataAdapter myDa = m_DbSession.CreateDataAdapter() as SqlDataAdapter;
            DataTable myTable = new DataTable();
            myDa.SelectCommand = m_Command;
            if (pFillSchema) myDa.FillSchema(myTable, SchemaType.Source);
            myDa.Fill(myTable);
            return myTable;
        }

        public void Execute(DataSet pDs, bool pFillSchema, string[] pTableNames)
        {
            
            Asserts.Assert(m_Command != null, "No current SqlCommand!");
            SqlDataAdapter myDa = m_DbSession.CreateDataAdapter() as SqlDataAdapter;
            myDa.SelectCommand = m_Command;
            if (pFillSchema) myDa.FillSchema(pDs, SchemaType.Source);
            myDa.Fill(pDs);
            if (pTableNames == null) return;

            int i = 0;
            foreach (string myName in pTableNames)
            {
                string myOriginalName = "Table";    //Tables names: Table, Table1, Table2, itd. http://msdn.microsoft.com/en-us/library/aa325442(v=vs.71).aspx
                if (i > 0) myOriginalName += i.ToString();
                pDs.Tables[myOriginalName].TableName = myName;
                i++;
            }
        }

        public DataTable ExecuteWithDataTable(string pTableName)
        {
            return ExecuteWithDataTable(pTableName, false);
        }

        public DataTable ExecuteWithDataTable(string pTableName, bool pFillSchema)
        {
            DataTable myTable = ExecuteWithDataTable(pFillSchema);
            myTable.TableName = pTableName;
            return myTable;
        }

        public object ExecuteScalar()
        {
            return m_Command.ExecuteScalar();
        }

        public DataTable GetSchema()
        {
            SqlDataAdapter myDa = m_DbSession.CreateDataAdapter() as SqlDataAdapter;
            DataTable myTable = new DataTable();
            myDa.SelectCommand = m_Command;
            myDa.FillSchema(myTable, SchemaType.Source);
            return myTable;
        }

        public DataTable GetSchema(string pReturnTableName)
        {
            DataTable myTable = GetSchema();
            myTable.TableName = pReturnTableName;
            return myTable;
        }

        public SqlParameter GetParam(String pName)
        {
            return m_Command.Parameters[pName];
        }

        public String GetResultString()
        {
            if (m_ReturnParam != "")
            {
                return m_Command.Parameters[m_ReturnParam].Value.ToString();
            }
            return "";
        }


        public string Name
        {
            get { return m_Name; }
        }

        public SqlCommand Command
        {
            get { return m_Command; }
        }

    }


}
