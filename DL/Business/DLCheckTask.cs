using ClassModel.TaskRelate;
using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public int UpdateStatusBatch(List<CheckTask> listCheckTask) {
            Dictionary<string, object> param = new Dictionary<string, object>();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i <= listCheckTask.Count; i++) {
                stringBuilder.Append($"UPDATE CheckTask SET Status = @Status{i} WHERE CheckTaskId = @CheckTaskId{i};");
                param.Add($"Status{i}", listCheckTask.ElementAt(i - 1).Status);
                param.Add($"CheckTaskId{i}", listCheckTask.ElementAt(i - 1).CheckTaskId);
            }

            var result = _dbConnection.Execute(stringBuilder.ToString(), param, commandType: System.Data.CommandType.Text);

            return result;
        }
    }
}
