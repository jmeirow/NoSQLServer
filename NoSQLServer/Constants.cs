using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQLServer
{
    public class Constants
    {

        public static readonly string PARAM_AGGREGATE_ID = "@AggregateID";
        public static readonly string PARAM_AGGREGATE_TYPE_ID = "@AggregateTypeID";
        public static readonly string PARAM_LOOKUP_VALUE = "@LookupValue";
        public static readonly string PARAM_DATA = "@Data";
        public static readonly string PARAM_COLUMN_NAMES = "@ColumnNames";
        public static readonly string PARAM_DATA_VALUES = "@DataValues";


        public static readonly string STORED_PROC_STRING = "{0}.p_{1}";


    }
}
