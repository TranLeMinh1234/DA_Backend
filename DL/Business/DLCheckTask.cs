using ClassModel.TaskRelate;
using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Business
{
    public class DLCheckTask : DLBase, IDLCheckTask
    {
        public List<CheckTask> GetCheckTasks(Guid taskId)
        {
            string sql = $"SELECT * FROM CheckTask WHERE TaskId = @TaskId;";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskId", taskId);
            var result = (List<CheckTask>)_dbConnection.Query<CheckTask>(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }
    }
}
