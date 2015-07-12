using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQLServer;


namespace DomainObjects.People
{
    [Serializable]
    public class Person : AggregateBase
    {

        private string _FirstName;
        public string FirstName
        {
            get
            {

                return _FirstName;
            }
            set
            {

                _FirstName = value;
            }
        }

        private string _LastName;
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
            }
        }



        private DateTime _BirthDate;
        public DateTime BirthDate
        {
            get
            {
                return _BirthDate;
            }
            set
            {
                _BirthDate = value;
            }
        }



        private string _Gender;
        public string Gender
        {
            get
            {
                return _Gender;
            }
            set
            {
                _Gender = value;
            }
        }



        private string _SSN;
        public string SSN
        {
            get
            {
                return _SSN;
            }
            set
            {
                _SSN = value;
            }
        }


        public static string ToStringHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('\r').Append('\n');
            sb.Append(string.Format(" {0,20}   {1,20}   {2,10}     {3,20}    {4,9}   {5,20}   {6,20}", "First Name", "Last Name", "Gender", "Birth Date", "SSN", "AggregateID", "AggregateTypeID"));
            sb.Append('\r').Append('\n');
            sb.Append(string.Format(" {0,20}   {1,20}   {2,10}     {3,20}    {4,9}   {5,20}   {6,20}", "==========", "===========", "=========", "==========", "========", "===========", "================"));
            return sb.ToString();

        }
        public override string ToString()
        {
            return string.Format(" {0,20}   {1,20}   {2,10}     {3,20}    {4,9}  {5,20}   {6,20}", FirstName, LastName, Gender, BirthDate.ToShortDateString(), SSN, AggregateID, AggregateTypeID);
        }


        public override string GetUniqueKey()
        {
            return SSN;
        }

        public override Dictionary<string, string> GetIndexes()
        {
            Dictionary<string, string> indexes = new Dictionary<string, string>();
            IndexData indexData = new IndexData();
            indexData.Add("LastName", LastName);
            indexData.Compute();
            indexes.Add(indexData.ColumnNames, indexData.DataValues);
            return indexes;
        }
    }
}
