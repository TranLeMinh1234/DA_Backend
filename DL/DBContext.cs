using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DL
{
    public class DBContext
    {
        string connectionString = "" +
                "Host = localhost;" +
                "Port= 3306;" +
                "Database = tlminhdb;" +
                "User Id = root;" +
                "Password = minhbeo2468";
        protected IDbConnection _dbConnection;

        public DBContext()
        {
            _dbConnection = new MySqlConnector.MySqlConnection(connectionString);
        }
    }
}
