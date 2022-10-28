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

        public Dictionary<string, object> GetPagingCustom(int from, int take, string searchValue) {
            string sql = string.Empty;
            if (string.IsNullOrEmpty(searchValue))
            {
                sql = "SELECT us.FirstName, us.LastName, us.Email, fa.FileName as FileAvatarName FROM User us INNER JOIN FileAttachment fa ON fa.AttachmentId = us.UserId ORDER BY LastName asc LIMIT @FROM,@Take;" +
                    "SELECT COUNT(*) FROM User us;";
            }
            else
            {
                sql = "SELECT us.FirstName, us.LastName, us.Email, fa.FileName as FileAvatarName FROM User us INNER JOIN FileAttachment fa ON fa.AttachmentId = us.UserId WHERE CONCAT(us.FirstName,' ',us.LastName) Like @SearchValue ORDER BY LastName asc LIMIT @FROM,@Take;" +
                    "SELECT COUNT(*) FROM User us WHERE CONCAT(us.FirstName,' ',us.LastName) Like @SearchValue;";
            }

            var param = new {
                SearchValue = $"%{searchValue}%",
                From = from,
                Take = take
            };

            var resultMuptiple = _dbConnection.QueryMultiple(sql, param, commandType: System.Data.CommandType.Text);
            var listUser = resultMuptiple.Read<ClassModel.User.User>().AsList();
            var countAllRecord = resultMuptiple.Read<int>().First();

            Dictionary<string,object> result = new Dictionary<string, object>();
            result.Add("listUserChoose", listUser);
            result.Add("numberOfRecords", countAllRecord);

            return result;
        }
    }
}
