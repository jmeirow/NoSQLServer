using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQLServer  
{
    public class IndexData
    {
        private Dictionary<string, string> values = new Dictionary<string, string>();

        public void Add(string columnName, int value) { values.Add(columnName, value.ToString()); }
        public void Add(string columnName, DateTime value) { values.Add(columnName, value.ToShortDateString()); }
        public void Add(string columnName, string value) { values.Add(columnName, value); }


        public void Compute()
        {
            List<string> keys = values.Keys.ToList<string>();
            List<string> valueList = new List<string>();

            keys.Sort();
            foreach (string key in keys)
            {
                valueList.Add(values[key]);
            }

            ColumnNames = string.Join("::", keys.ToArray<string>());
            DataValues = string.Join("::", valueList.ToArray<string>());
        }

        public string ColumnNames { get; private set; }
        public string DataValues { get; private set; }
    }
}
