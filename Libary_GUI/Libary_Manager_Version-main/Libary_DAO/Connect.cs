using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libary_Manager.Libary_DAO
{
    public class Connect
    {
        private static Connect _instance;
        protected static SqlConnection _conn;

        static Connect()
        {
            _conn = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=Libary_Manager;Integrated Security=True");
        }

        public static Connect Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Connect();
                }
                return _instance;
            }
        }

        public SqlConnection GetConnection()
        {
            return _conn;
        }
    }
}
