using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Business
{
    public class DLLogin : DLBase, IDLLogin
    {
        public bool checkEmailRegisterExists(string Email)
        {
            string sql = $"SELECT EXISTS(SELECT * FROM USER WHERE Email = @Email);";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Email", Email);
            var result = _dbConnection.QueryFirst<Boolean>(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }
    }
}
