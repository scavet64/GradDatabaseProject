using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Kinabalu.DAL
{
    public class ApplicationDb : IDisposable
    {
        public MySqlConnection Connection;

        public ApplicationDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public ApplicationDb()
        {
            string test = AppConfig.Config["ConnectionStrings:DefaultConnection"];
            Connection = new MySqlConnection(AppConfig.Config["ConnectionStrings:DefaultConnection"]);
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
