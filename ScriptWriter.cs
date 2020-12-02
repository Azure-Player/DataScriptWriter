using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace DataScriptWriter
{
    public class ScriptWriter
    {
        private const int MaxRowsPerBatch = 1000;
        private CAMOsoft.DbUtils.MsSqlSession _db = null;
        private string _OutputFolder = "";
        private DataRow _ServerInfoRow = null;
        private DataTable _dt;
        DataView _dv;
        public bool OptionProcWrapUp = false;
        private string _BatchSeparator = "GO";

        public ScriptWriter(CAMOsoft.DbUtils.MsSqlSession db, string outputFolder)
        {
            _db = db;
            _OutputFolder = outputFolder;
            string sql = "SELECT @@SPID AS SPID, SUSER_NAME() AS UserName, DB_NAME() AS DbName, @@SERVERNAME AS ServerName, @@VERSION as ServerVersion;";
            _ServerInfoRow = _db.SelectRow(sql);
            InitTable();
        }

        private void InitTable()
        {
            _dt = new DataTable();
            _dt.Columns.Add("id", typeof(System.String));
            _dt.Columns.Add("isSelected", typeof(System.Boolean));
            _dt.Columns.Add("schema", typeof(System.String));
            _dt.Columns.Add("table", typeof(System.String));
            _dt.Columns.Add("method", typeof(System.String));
            _dt.Columns.Add("rowcount", typeof(System.Int64));
            _dv = new DataView(_dt, "isSelected=1", "", DataViewRowState.CurrentRows);
        }


        public void LoadListOfTables()
        {
            string sql = Properties.Resources.ResourceManager.GetString("LoadListOfTables");
            DataTable TableList = _db.SelectTable(sql, "TableList");
            foreach (DataRow dr in TableList.Rows)
            {
                AddItemToTable(dr["SchemaName"].ToString(), dr["TableName"].ToString(), "MERGE", Int64.Parse(dr["RowCnt"].ToString()));
            }
        }

        private void AddItemToTable(string schema, string table, string method, long rowCount)
        {
            DataRow newRow = _dt.NewRow();
            newRow["id"] = String.Format("{0}.{1}", schema, table);
            newRow["schema"] = schema;
            newRow["table"] = table;
            newRow["method"] = method;
            newRow["isSelected"] = false;
            newRow["rowcount"] = rowCount;
            _dt.Rows.Add(newRow);
        }

        private string GetColList(DataTable dt, string filter, string format)
        {
            List<string> cols = new List<string>();
            foreach (DataRow row in dt.Select(filter))
            {
                string s = String.Format(format, row["Q_COLUMN_NAME"].ToString());
                cols.Add(s);
            }
            return String.Join(", ", cols);
        }
        private string GetColListWithCastForPK(DataTable dt)
        {
            List<string> cols = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                string s = "";
                if (row["constraint_type"].ToString() == "PRIMARY KEY")
                    s = String.Format("CAST({0} AS {1}) AS {0}", row["Q_COLUMN_NAME"], row["DATA_TYPE"]);
                else
                    s = String.Format("{0}", row["Q_COLUMN_NAME"].ToString());
                cols.Add(s);
            }
            return String.Join(", ", cols);
        }

        private DataTable LoadColumnsInfo(ScriptObject so)
        {
            string queryDef = "LoadColumnInfo";
            if (IsSQLServer2016orLater()) queryDef = "LoadColumnInfo2016andLater";
            string sql = Properties.Resources.ResourceManager.GetString(queryDef);
            sql = sql.Replace("{0}", so.FullName);
            DataTable dt = _db.SelectTable(sql, "ColumnInfo");
            return dt;
        }

        private bool IsSQLServer2016orLater()
        {
            return (this.ServerVersion.StartsWith("Microsoft SQL Azure") || this.ServerVersion.CompareTo("Microsoft SQL Server 2016") > 0);
        }

        private StringBuilder SerializeRowValues(DataRow row, DataTable colInfoTable, string prefix, string suffix)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
            bool first = true;
            foreach (DataColumn col in row.Table.Columns)
            {
                if (!first)
                {
                    sb.Append(",\t");
                }
                first = false;
                string sqltype = colInfoTable.Select("COLUMN_NAME = '" + col.ColumnName + "'")[0]["DATA_TYPE"].ToString();
                string v = "";
                if (row[col] == DBNull.Value)
                {
                    v = "null";
                }
                else
                {
                    switch (sqltype)
                    {
                        case "uniqueidentifier":
                            v = "'" + row[col].ToString() + "'";
                            break;
                        case "xml":
                            v = "'" + row[col].ToString().Replace("'", "''") + "'";
                            break;
                        case "char":
                        case "varchar":
                        case "text":
                            v = "'" + row[col].ToString().Replace("'", "''") + "'";
                            break;
                        case "nchar":
                        case "nvarchar":
                        case "ntext":
                        case "sysname":
                            v = "N'" + row[col].ToString().Replace("'", "''") + "'";
                            break;
                        case "datetime":
                            v = String.Format("'{0:yyyyMMdd HH:mm:ss}'", row[col]);     //Lack of accuracy!
                            break;
                        case "date":
                            v = String.Format("'{0:yyyyMMdd}'", row[col]);
                            break;
                        case "tinyint":
                        case "int":
                        case "float":
                        case "numeric":
                        case "decimal":
                        case "smallmoney":
                        case "money":
                        case "smallint":
                        case "real":
                        case "bigint":
                            v = String.Format("{0}", row[col]);
                            break;
                        case "bit":
                            v = ((bool)row[col]) == true ? "1" : "0";
                            break;
                        case "binary":
                        case "varbinary":
                            v = "0x" + ByteArrayToHex((byte[])row[col]);
                            break;
                        default:
                            throw new Exception("Unknown SQL data type! (" + sqltype + ")");
                    }
                }
                sb.Append(v);
            }
            sb.Append(suffix);
            return sb;
        }

        private string ByteArrayToHex(byte[] barray)
        {
            char[] c = new char[barray.Length * 2];
            byte b;
            for (int i = 0; i < barray.Length; ++i)
            {
                b = ((byte)(barray[i] >> 4));
                c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(barray[i] & 0xF));
                c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }

        private string getSelectStatement(DataTable colInfoTable, ScriptObject so)
        {
            StringBuilder sb = new StringBuilder("SELECT ");
            int RowIndex = 0;
            foreach (DataRow row in colInfoTable.Rows)
            {
                string dt = row["DATA_TYPE"].ToString();
                string colName = row["Q_COLUMN_NAME"].ToString();
                switch (dt)
                {
                    case "datetime":
                    case "datetime2":
                        sb.AppendFormat("REPLACE(CONVERT(varchar(50), {0}, 121), '-', '') as {0}", colName);
                        colInfoTable.Columns["DATA_TYPE"].ReadOnly = false;
                        row["DATA_TYPE"] = "varchar";
                        break;
                    default:
                        sb.Append(colName);
                        break;
                }
                RowIndex++;
                if (RowIndex < row.Table.Rows.Count)
                    sb.Append(", ");
            }
            sb.Append(" FROM " + so.FullQuoted + ";");
            return sb.ToString();
        }

        private bool hasIdentity(DataTable colInfoTable)
        {
            return colInfoTable.Select("is_identity = 1").Length > 0;
        }

        public void GenerateForTable(ScriptObject so)
        {
            _BatchSeparator = this.OptionProcWrapUp ? "" : "GO";
            DataTable colInfoTable = LoadColumnsInfo(so);
            DataTable mainTable = _db.SelectTable(getSelectStatement(colInfoTable, so), "MainTable");

            string filename = Path.Combine(_OutputFolder, so.FullName + ".sql");
            System.IO.StreamWriter w = new System.IO.StreamWriter(filename);

            string colList = GetColList(colInfoTable, "", "{0}");
            string colCastList = GetColListWithCastForPK(colInfoTable);
            string scolList = GetColList(colInfoTable, "", "s.{0}");
            string PKcolListOn = GetColList(colInfoTable, "constraint_type = 'PRIMARY KEY'", "t.{0} = s.{0}");

            //SP
            if (OptionProcWrapUp)
            {
                w.WriteLine(String.Format("CREATE PROCEDURE [{0}].[Populate_{1}]", "dbo", so.TempCounterpart.Replace("#", "")));
                w.WriteLine("AS");
                w.WriteLine("BEGIN");
            }

            //Header
            w.WriteLine("/*");
            w.WriteLine(String.Format("\tTable's data:    {0}", so.FullQuoted));
            w.WriteLine(String.Format("\tData Source:     [{0}].[{1}]", ServerName, DbName));
            w.WriteLine(String.Format("\tCreated on:      {0}", DateTime.Now));
            w.WriteLine(String.Format("\tScripted by:     {0}", UserName));
            w.WriteLine(              "\tGenerated by:    " + Global.AppName);
            w.WriteLine(              "\tGitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/");
            w.WriteLine("*/");
        
            //Body
            w.WriteLine(String.Format("PRINT 'Populating data into {0}';", so.FullQuoted));
            w.WriteLine(_BatchSeparator);

            switch (so.Method)
            {
                case "MERGE":
                case "MERGE_without_DELETE":
                case "MERGE_NEW_ONLY":
                    ScriptTableMerge(so, colInfoTable, mainTable, w, colList, scolList, PKcolListOn, colCastList);
                    break;
                case "INSERT":
                    ScriptTableInitialInsert(so, colInfoTable, mainTable, w, colList, scolList, PKcolListOn);
                    break;
                default:
                    throw new Exception("Unknown method: " + so.Method);
            }

            //Footer
            w.WriteLine(String.Format("-- End data of table: {0} --", so.FullQuoted));
            if (OptionProcWrapUp)
            {
                w.WriteLine("END");
                w.WriteLine("GO");
            }

            w.Close();
            Console.WriteLine(so.FullQuoted + " was scripted.");
        }

        private void ScriptTableMerge(ScriptObject so, DataTable colInfoTable, DataTable mainTable, System.IO.StreamWriter w, string colList, string scolList, string PKcolListOn, string colCastList)
        {
            string tmpTableName = so.TempCounterpart;
            bool useIdentity = hasIdentity(colInfoTable);

            w.WriteLine(String.Format("IF OBJECT_ID('tempdb.dbo.{0}') IS NOT NULL DROP TABLE {0};", tmpTableName));
            w.WriteLine(String.Format("SELECT {2} INTO {1} FROM {0} WHERE 0=1;", so.FullQuoted, tmpTableName, colList));
            w.WriteLine(_BatchSeparator);

            if (useIdentity)
            {
                w.WriteLine(String.Format("SET IDENTITY_INSERT {0} ON;", tmpTableName));
                w.WriteLine(_BatchSeparator);
            }

            string rowsep = "\t  ";
            int rowIndex = 0;
            foreach (DataRow row in mainTable.Rows)
            {
                //Begin Insert to temp
                if (rowIndex % MaxRowsPerBatch == 0)
                {
                    w.WriteLine(String.Format("INSERT INTO {0} ", tmpTableName));
                    w.WriteLine(String.Format(" ({0})", colList));
                    w.WriteLine(String.Format("SELECT {0} FROM ", colCastList));
                    w.WriteLine("(VALUES");
                    rowsep = "\t  ";
                }

                //VALUES
                w.Write(rowsep);
                rowsep = "\t, ";
                w.WriteLine(SerializeRowValues(row, colInfoTable, "(", ")"));
                rowIndex++;

                //End Insert to Temp
                if (rowIndex % MaxRowsPerBatch == 0 || rowIndex == mainTable.Rows.Count)
                {
                    w.WriteLine(String.Format(") as v ({0});", colList));
                    w.WriteLine(_BatchSeparator);
                    w.WriteLine("");
                }
            }

            //Identity
            if (useIdentity)
            {
                w.WriteLine(String.Format("SET IDENTITY_INSERT {0} OFF;", tmpTableName));
                w.WriteLine(_BatchSeparator);
                w.WriteLine(String.Format("SET IDENTITY_INSERT {0} ON;", so.FullQuoted));
                w.WriteLine(_BatchSeparator);
            }

            //Begin Merge
            w.WriteLine("");
            w.WriteLine(String.Format("WITH cte_data as (SELECT {1} FROM [{0}])", tmpTableName, colCastList));
            w.WriteLine(String.Format("MERGE {0} as t", so.FullQuoted));
            w.WriteLine("USING cte_data as s");
            w.WriteLine(String.Format("\tON {0}", PKcolListOn.Replace(",", " AND")));
            w.WriteLine("WHEN NOT MATCHED BY target THEN");
            w.WriteLine(String.Format("\tINSERT ({0})", colList));
            w.WriteLine(String.Format("\tVALUES ({0})", scolList));

            //Merge: WHEN MATCHED
            if (so.Method != "MERGE_NEW_ONLY")
            {
                string UpdateColList = GetColList(colInfoTable, "constraint_type IS NULL", "{0} = s.{0}");
                w.WriteLine("WHEN MATCHED THEN ");
                w.WriteLine("\tUPDATE SET ");
                w.WriteLine(String.Format("\t{0}", UpdateColList));
            }

            //Merge: WHEN NOT MATCHED
            if (so.Method == "MERGE")
            {
                w.WriteLine("WHEN NOT MATCHED BY source THEN");
                w.WriteLine("\tDELETE");
            }

            w.WriteLine(";");
            w.WriteLine(_BatchSeparator);

            //End
            if (useIdentity)
            {
                w.WriteLine(String.Format("SET IDENTITY_INSERT {0} OFF;", so.FullQuoted));
                w.WriteLine(_BatchSeparator);
            }
            w.WriteLine(String.Format("DROP TABLE {0};", tmpTableName));
            w.WriteLine(_BatchSeparator);
        }

        private void ScriptTableInitialInsert(ScriptObject so, DataTable colInfoTable, DataTable mainTable, System.IO.StreamWriter w, string colList, string scolList, string PKcolListOn)
        {
            bool useIdentity = hasIdentity(colInfoTable);

            //Begin IF
            w.WriteLine(String.Format("IF NOT EXISTS (SELECT TOP (1) * FROM {0})", so.FullQuoted));
            w.WriteLine("BEGIN");
            w.WriteLine("");
            if (useIdentity)
            {
                w.WriteLine(String.Format("\tSET IDENTITY_INSERT {0} ON;", so.FullQuoted));
                w.WriteLine("");
            }

            w.WriteLine("\t;WITH cte_data");
            w.WriteLine(String.Format("\tas (SELECT {0} FROM ", colList));
            w.WriteLine("\t(VALUES");

            //VALUES
            string rowsep = "\t  ";
            foreach (DataRow row in mainTable.Rows)
            {
                w.Write(rowsep);
                rowsep = "\t, ";
                w.WriteLine(SerializeRowValues(row, colInfoTable, "(", ")"));
            }

            w.WriteLine(String.Format("\t) as v ({0})", colList));
            w.WriteLine(")");

            w.WriteLine(String.Format("\tINSERT INTO {0}", so.FullQuoted));
            w.WriteLine(String.Format("\t({0})", colList));
            w.WriteLine(String.Format("\tSELECT {0}", colList));
            w.WriteLine("\tFROM cte_data;");

            //End 
            if (useIdentity)
            {
                w.WriteLine("");
                w.WriteLine(String.Format("\tSET IDENTITY_INSERT {0} OFF;", so.FullQuoted));
            }

            w.WriteLine("");
            w.WriteLine("END");
            w.WriteLine(_BatchSeparator);
        }


        public string UserName {
            get
            {
                return _ServerInfoRow["UserName"].ToString();
            }
        }
        public string DbName
        {
            get
            {
                return _ServerInfoRow["DbName"].ToString();
            }
        }

        public string ServerName
        {
            get
            {
                return _ServerInfoRow["ServerName"].ToString();
            }
        }

        public string ServerVersion
        {
            get
            {
                return _ServerInfoRow["ServerVersion"].ToString();
            }
        }

        public DataTable TableList
        {
            get
            {
                return _dt;
            }
        }

        public DataView SelectedItemView
        {
            get
            {
                return _dv;
            }
        }

    }
}
