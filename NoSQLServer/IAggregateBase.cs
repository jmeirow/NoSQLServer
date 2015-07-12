using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQLServer
{
    public interface IAggregateBase
    {
        long AggregateID { get; set; }
        long AggregateTypeID { get; set; }
        int VersionNumber { get; set;  }
    }
}
