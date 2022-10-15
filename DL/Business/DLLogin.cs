using ClassModel.User;
using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL.Business
{
    public class DLLogin : DLBase, IDLLogin
    {
        public bool checkEmailRegisterExists(string email)
        {
            string sql = $"SELECT EXISTS(SELECT * FROM USER WHERE Email = @Email);";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Email", email);
            var result = _dbConnection.Query<Boolean>(sql, param, commandType: System.Data.CommandType.Text).FirstOrDefault();
            return result;
        }

        public User GetUserByEmail(string email)
        {
            string sql = $"SELECT * FROM USER WHERE Email = @Email;";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Email", email);
            var result = _dbConnection.Query<User>(sql, param, commandType: System.Data.CommandType.Text).FirstOrDefault();
            return result;
        }
    }
}
