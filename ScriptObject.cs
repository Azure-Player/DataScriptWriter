using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataScriptWriter
{
    public class ScriptObject
    {

        private string _schema;
        private string _table;
        private string _method;

        public ScriptObject(DataRowView row)
        {
            _schema = Plain(row["schema"].ToString());
            _table = Plain(row["table"].ToString());
            _method = row["method"].ToString();
        }

        private string Plain(string name)
        {
            return name.Replace("[", "").Replace("]", "");
        }

        public string Schema { get => _schema; }
        public string Table { get => _table; }
        public string Method { get => _method; }
        public string FullName { get => _schema + "." + _table; }
        public string FullQuoted { get => "[" + _schema + "].[" + _table + "]"; }
        public string TempCounterpart { get => "#" + FullName.Replace(".", "_").Replace(" ", "_"); }

    }
}
