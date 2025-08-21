using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProjectAccount
{
    public class Connection
    {
        protected string conLine { get; set; }
        public MySqlConnection con { get; set; }

        public Connection()
        {
            conLine = "SERVER =127.0.0.1 ; port = 3306; datasource = localhost; username = root; database = project;";
            con = new MySqlConnection(conLine);
        }

        public void OpenConnection()
        {
            if (con.State != System.Data.ConnectionState.Open)
            {
                con.Open();
            }
        }

        public void CloseConnection()
        {
            if (con.State != System.Data.ConnectionState.Closed)
            {
                con.Close();
            }
        }
    }
}
