using ClassModel.File;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using ClassModel.User;
using Dapper;
using DL.Interface;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static ClassModel.Enumeration;

namespace DL.Business
{
    public class DLTask : DLBase, IDLTask
    {
        public Task GetLastTask(string email, int typeTask, Guid? groupTaskId = null, Guid? processId = null)
        {
            string sql = string.Empty;
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TypeTask", typeTask);
            if (typeTask == (int)EnumTypeTask.Personal)
            {
                sql = $"SELECT * FROM Task WHERE CreatedByEmail = @Email AND TypeTask = @TypeTask ORDER BY SortOrder LIMIT 0,1;";
                param.Add("Email", email);
            }
            else
            {
                sql = $"SELECT * FROM Task WHERE GroupTaskId = @GroupTaskId AND ProcessId = @ProcessId AND TypeTask = @TypeTask ORDER BY SortOrder LIMIT 0,1;";
                param.Add("GroupTaskId", groupTaskId);
                param.Add("ProcessId", processId);
            }
            var result = _dbConnection.Query<Task>(sql, param, commandType: System.Data.CommandType.Text).FirstOrDefault();
            return result;
        }

        public List<Task> GetChildTask(Guid taskId)
        {
            string sql = $"SELECT * FROM Task WHERE PathTreeTask like @TaskId;";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskId", "%" + taskId.ToString());
            var result = (List<Task>)_dbConnection.Query<Task>(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public int InsertLabelsTask(Guid taskId, List<string> listLabelId)
        {
            StringBuilder sql = new StringBuilder();
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskId", taskId);
            for (int i = 0; i < listLabelId.Count; i++)
            {
                sql.Append($"INSERT INTO TaskLabel (TaskId, LabelId) values (@TaskId,@LabelId{i});");
                param.Add($"LabelId{i}", listLabelId.ElementAt(i));
            }
            var result = _dbConnection.Execute(sql.ToString(), param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public List<Label> GetLabelsTask(Guid taskId)
        {
            string sql = $"SELECT * FROM Label ll INNER JOIN TaskLabel tl ON tl.LabelId = ll.LabelId AND tl.TaskId = @TaskId";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskId", taskId);
            var result = (List<Label>)_dbConnection.Query<Label>(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public int DeleteLabelsTask(Guid taskId, Guid labelId)
        {
            string sql = $"DELETE FROM TaskLabel Where TaskId = @TaskId AND LabelId = @LabelId";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskId", taskId);
            param.Add("LabelId", labelId);
            var result = _dbConnection.Execute(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public void GetCommentsTask(Guid taskId, out List<Comment> comments, out List<FileAttachment> fileAttachments, out List<User> users)
        {
            //var query = "Drop Table if exists CommentTask;\r\n\r\n\tCREATE TEMPORARY TABLE CommentTask SELECT * FROM Comment Where AttachmentId = @TaskId;\r\n\r\n\tSelect * From CommentTask;\r\n\tSelect fa.* From fileattachment fa INNER JOIN CommentTask ct ON fa.AttachmentId = ct.CommentId;";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskId", taskId);
            var resultExecutor = _dbConnection.QueryMultiple("Proc_GetCommentsTask", new { TaskId = taskId }, null, null, CommandType.StoredProcedure);
            comments = resultExecutor.Read<Comment>().ToList();
            users = resultExecutor.Read<User>().ToList();
            fileAttachments = resultExecutor.Read<FileAttachment>().ToList();
        }

        public int UpdateDescription(Guid taskId, string description)
        {
            string sql = $"UPDATE Task SET Description = @Description WHERE TaskId = @TaskId";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskId", taskId);
            param.Add("Description", description);
            var result = _dbConnection.Execute(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public Task GetFullInfo(Guid taskId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskIdParam", taskId);
            var result = _dbConnection.Query<Task, User, User, User, Task>("Proc_GetFullInfoTask",
                map: (task, userDoTask, userAssign, userCreate) =>
                {
                    task.AssignedBy = userAssign;
                    task.AssignedFor = userDoTask;
                    task.CreatedBy = userCreate;

                    return task;
                },
                param,
                splitOn: "Email,Email,Email",
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }

        public List<Task> GetDailyTask(ParamDailyTask paramDailyTask, string email)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            Dictionary<Guid, Task> taskMap = new Dictionary<Guid, Task>();
            param.Add("StartTimeQuery", paramDailyTask.StartTime);
            param.Add("EndTimeQuery", paramDailyTask.EndTime);
            param.Add("EmailQuery", email);

            var result = (List<Task>)_dbConnection.Query<Task, User, Label, Task>("Proc_GetDailyTask",
                (task, userDoTask, label) =>
                {
                    if (taskMap.ContainsKey((Guid)task.TaskId))
                    {
                        taskMap.TryGetValue((Guid)task.TaskId, out task);
                    }
                    else
                    {
                        taskMap.Add((Guid)task.TaskId, task);
                    }

                    if (task != null)
                    {
                        if (label != null)
                            task.ListLabel.Add(label);

                        task.AssignedFor = userDoTask;
                    }

                    return task;
                },
                param,
                splitOn: "Email,LabelId", commandType: System.Data.CommandType.StoredProcedure);
            return (List<Task>)taskMap.Values.ToList();
        }

        public int DeleteCustom(Guid taskId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskIdQuery", taskId);
            var result = _dbConnection.Execute("Proc_DeleteTask", param, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public int UpdateDeadline(string deadlineUpdate, DateTime? newDeadline, Guid taskId)
        {
            string sql = $"UPDATE Task SET {deadlineUpdate} = @NewDeadline Where TaskId = @TaskIdQuery; DELETE FROM reminddatastore WHERE TaskId = @TaskIdQuery";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskIdQuery", taskId);
            param.Add("NewDeadline", newDeadline);
            var result = _dbConnection.Execute(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public int InsertRemindDataStore(RemindDataStore remindDataStore)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("RemindDataStoreIdSql", remindDataStore.RemindDataId);
            param.Add("TypeRemindSql", remindDataStore.TypeRemind);
            param.Add("EmailRemindedUserSql", remindDataStore.EmailRemindedUser);
            param.Add("TaskIdSql", remindDataStore.TaskId);
            param.Add("IsUsedSql", remindDataStore.IsUsed);

            var result = _dbConnection.Execute("Proc_InsertRemindDataStore", param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public int UpdateTaskProcessBatch(List<ParamUpdateTaskProcessBatch> listParam) {
            Dictionary<string, object> param = new Dictionary<string, object>();
            StringBuilder sqlBuilder = new StringBuilder();

            for (int i = 1; i <= listParam.Count;i++)
            {
                param.Add($"TaskQueryId{i}", listParam.ElementAt(i-1).TaskId);
                param.Add($"ProcessQueryId{i}", listParam.ElementAt(i-1).ProcessId);
                param.Add($"SortOrderQueryId{i}", listParam.ElementAt(i-1).SortOrder);
                sqlBuilder.Append($"UPDATE Task SET ProcessId = @ProcessQueryId{i}, SortOrder = @SortOrderQueryId{i} WHERE TaskId = @TaskQueryId{i};");
            }

            var result = _dbConnection.Execute(sqlBuilder.ToString(), param, commandType: CommandType.Text);
            return result;
        }
    }
}
