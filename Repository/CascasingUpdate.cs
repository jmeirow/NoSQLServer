using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQLServer;


namespace NoSQLServer
{
    //[Serializable]
    //public class CascasingUpdate : AggregateBase
    //{

    //    private long _Source = 0;
    //    private long _Target = 0;
    //    public long Source 
    //    {
    //        get { return _Source; }
    //        set { _Source = value; }
    //    }

    //    public long Target
    //    {
    //        get { return _Target; }
    //        set { _Target = value; }
    //    }


    //    public CascasingUpdate(AggregateBase source, AggregateBase target) 
    //    {
    //        Source = AggregateType.GetInfo(source.GetType().FullName).ID;
    //        Target = AggregateType.GetInfo(target.GetType().FullName).ID;
    //    }



    //    public override Dictionary<string, string> GetIndexes()
    //    {
    //        return  new Dictionary<string, string>();
    //    }
    //}
}
