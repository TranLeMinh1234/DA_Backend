using ClassModel.TaskRelate;
using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL.Business
{
    public class DLTask : DLBase, IDLTask
    {
        public Task GetLastTask(string email)
        {
            string sql = $"SELECT * FROM Task WHERE CreatedByEmail = @Email AND TypeTask = 1 ORDER BY SortOrder LIMIT 0,1;";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Email", email);
            var result = _dbConnection.Query<Task>(sql, param, commandType: System.Data.CommandType.Text).FirstOrDefault();
            return result;
        }

        public List<Task> GetChildTask(Guid taskId)
        {
            string sql = $"SELECT * FROM Task WHERE PathTreeTask like @TaskId;";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskId", "%"+taskId.ToString());
            var result = (List<Task>)_dbConnection.Query<Task>(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }
    }
}
