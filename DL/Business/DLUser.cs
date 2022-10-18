using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL.Business
{
    public class DLUser : DLBase, IDLUser
    {
        public ClassModel.User.User GetUserInfo(string email) {
            string sql = $"SELECT us.*,fa.FileName as FileAvatarName FROM User us INNER JOIN FileAttachment fa ON us.UserId = fa.AttachmentId WHERE Email = @Email;";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Email", email);
            var result = _dbConnection.Query<ClassModel.User.User>(sql, param, commandType: System.Data.CommandType.Text).FirstOrDefault();
            return result;
        }
    }
}
