using ClassModel.TaskRelate;
using ClassModel.User;
using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL.Business
{
    public class DLGroupTask : DLBase, IDLGroupTask
    {
        public int InsertBatchJoinedGroupTask(List<JoinedGroupTask> joinedGroupTasks) {
            StringBuilder sqlValuesInsert = new StringBuilder();
            Dictionary<string, object> param = new Dictionary<string, object>();
            for (int i = 1; i <= joinedGroupTasks.Count; i++)
            {
                sqlValuesInsert.Append($"(@JoinId{i}, @UserJoinedEmail{i}, @InvitedByEmail{i}, @RoleReferenceId{i}, @GroupTaskReferenceId{i}, @JoinedTime{i}),");
                param.Add($"JoinId{i}",Guid.NewGuid());
                param.Add($"UserJoinedEmail{i}", joinedGroupTasks.ElementAt(i-1).UserJoinedEmail);
                param.Add($"InvitedByEmail{i}", joinedGroupTasks.ElementAt(i - 1).InvitedByEmail);
                param.Add($"RoleReferenceId{i}", joinedGroupTasks.ElementAt(i - 1).RoleReferenceId);
                param.Add($"GroupTaskReferenceId{i}", joinedGroupTasks.ElementAt(i - 1).GroupTaskReferenceId);
                param.Add($"JoinedTime{i}", joinedGroupTasks.ElementAt(i - 1).JoinedTime);
            }

            sqlValuesInsert = sqlValuesInsert.Remove(sqlValuesInsert.Length-1,1);

            string sql = $"INSERT INTO JoinedGroupTask (JoinId,UserJoinedEmail,InvitedByEmail,RoleReferenceId,GroupTaskReferenceId,JoinedTime) VALUES {sqlValuesInsert.ToString()};";

            var result = _dbConnection.Execute(sql,param,commandType: System.Data.CommandType.Text);
            return result;
        }

        public Dictionary<string, object> GetGroupTaskHaveJoined(string email) {

            var resultMuptile = _dbConnection.QueryMultiple("Proc_GetGroupTaskHaveJoined", new
            {
                EmailQuery = email
            }, commandType: System.Data.CommandType.StoredProcedure);

            var lstGroupPersonalTask = resultMuptile.Read<GroupTask>().AsList();
            var lstGroupCommunityTask = resultMuptile.Read<GroupTask>().AsList();

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("lstGroupPersonalTask", lstGroupPersonalTask);
            result.Add("lstGroupCommunityTask", lstGroupCommunityTask);

            return result;
        }

        public List<ClassModel.User.User> GetUserJoined(Guid groupTaskId) {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("GroupTaskQueryId", groupTaskId);

            var result = _dbConnection.Query<User>("Proc_GetUserJoined", param, commandType: System.Data.CommandType.StoredProcedure);
            return (List<User>)result;
        }

        public TemplateGroupTask GetInfoTemplate(Guid groupTaskId, Guid templateReferenceId) {
            Dictionary<string, TemplateGroupTask> resultMapping = new Dictionary<string, TemplateGroupTask>();
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("GroupTaskQueryId", groupTaskId);
            param.Add("TemplateReferenceQueryId", templateReferenceId);


            var result = _dbConnection.Query<TemplateGroupTask, Process, ColumnSetting, TemplateGroupTask>("Proc_GetInfoTemplate", 
                (templateGroupTask, process, columnSetting) => {

                    process.ColumnSetting = columnSetting;

                    if (resultMapping.ContainsKey(templateGroupTask.TemplateGroupTaskId.ToString()))
                    {
                        if (resultMapping.TryGetValue(templateGroupTask.TemplateGroupTaskId.ToString(), out templateGroupTask))
                        {
                            templateGroupTask.ListProcess.Add(process);
                        }
                    }
                    else
                    {
                        templateGroupTask.ListProcess.Add(process);
                        resultMapping.Add(templateGroupTask.TemplateGroupTaskId.ToString(), templateGroupTask);
                    }

                    return templateGroupTask;
                },
                param,
                splitOn: "ProcessId,ColumnSettingId", commandType: System.Data.CommandType.StoredProcedure);

            return resultMapping.Values.AsList().ElementAt(0);
        }

        public List<Task> GetAllTask(Guid groupTaskId) {
            Dictionary<Guid, Task> taskMap = new Dictionary<Guid, Task>();
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("GroupTaskQueryId", groupTaskId);

            var result = (List<Task>)_dbConnection.Query<Task, User, Label, Task>("Proc_GetTasksGroupTask",
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
    }
}
