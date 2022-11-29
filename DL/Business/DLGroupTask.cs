using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using ClassModel.User;
using Dapper;
using DL.Interface;
using Org.BouncyCastle.Crypto.Tls;
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
                param.Add($"JoinId{i}", Guid.NewGuid());
                param.Add($"UserJoinedEmail{i}", joinedGroupTasks.ElementAt(i - 1).UserJoinedEmail);
                param.Add($"InvitedByEmail{i}", joinedGroupTasks.ElementAt(i - 1).InvitedByEmail);
                param.Add($"RoleReferenceId{i}", joinedGroupTasks.ElementAt(i - 1).RoleReferenceId);
                param.Add($"GroupTaskReferenceId{i}", joinedGroupTasks.ElementAt(i - 1).GroupTaskReferenceId);
                param.Add($"JoinedTime{i}", joinedGroupTasks.ElementAt(i - 1).JoinedTime);
            }

            sqlValuesInsert = sqlValuesInsert.Remove(sqlValuesInsert.Length - 1, 1);

            string sql = $"INSERT INTO JoinedGroupTask (JoinId,UserJoinedEmail,InvitedByEmail,RoleReferenceId,GroupTaskReferenceId,JoinedTime) VALUES {sqlValuesInsert.ToString()};";

            var result = _dbConnection.Execute(sql, param, commandType: System.Data.CommandType.Text);
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

            var result = _dbConnection.Query<User, Role, User>("Proc_GetUserJoined",
                (user, role) => {
                    user.Role = role;
                    return user;
                }
                , param,
                splitOn: "RoleId", commandType: System.Data.CommandType.StoredProcedure);
            return (List<User>)result;
        }

        public TemplateGroupTask GetInfoTemplateOrigin(Guid templateReferenceId) {
            Dictionary<string, TemplateGroupTask> resultMapping = new Dictionary<string, TemplateGroupTask>();
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TemplateReferenceQueryId", templateReferenceId);


            var result = _dbConnection.Query<TemplateGroupTask, Process, ColumnSetting, TemplateGroupTask>("Proc_GetInfoTemplateOrigin",
                (templateGroupTask, process, columnSetting) => {

                    process.ColumnSetting = columnSetting;
                    process.ColumnSettingReferenceId = columnSetting.ColumnSettingId;

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

        public TemplateCustom GetInfoTemplate(Guid groupTaskId, Guid templateReferenceId) {
            Dictionary<string, TemplateCustom> resultMapping = new Dictionary<string, TemplateCustom>();
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("GroupTaskQueryId", groupTaskId);
            param.Add("TemplateReferenceQueryId", templateReferenceId);


            var result = _dbConnection.Query<TemplateCustom, Process, ColumnSetting, TemplateCustom>("Proc_GetInfoTemplate",
                (templateCustom, process, columnSetting) => {

                    process.ColumnSetting = columnSetting;

                    if (resultMapping.ContainsKey(templateCustom.TemplateCustomId.ToString()))
                    {
                        if (resultMapping.TryGetValue(templateCustom.TemplateCustomId.ToString(), out templateCustom))
                        {
                            templateCustom.ListProcess.Add(process);
                        }
                    }
                    else
                    {
                        templateCustom.ListProcess.Add(process);
                        resultMapping.Add(templateCustom.TemplateCustomId.ToString(), templateCustom);
                    }

                    return templateCustom;
                },
                param,
                splitOn: "ProcessId,ColumnSettingId", commandType: System.Data.CommandType.StoredProcedure);

            return resultMapping.Values.AsList().ElementAt(0);
        }

        public List<Task> GetAllTask(ParamGetAllTask paramGetAllTask) {
            Dictionary<Guid, Task> taskMap = new Dictionary<Guid, Task>();
            Dictionary<string, object> param = new Dictionary<string, object>();

            StringBuilder sql = new StringBuilder();
            string sqlCondition = string.Empty;

            if (paramGetAllTask.GroupTaskId != null)
            {
                sqlCondition += "GroupTaskId = @GroupTaskQueryId";
                param.Add("GroupTaskQueryId", paramGetAllTask.GroupTaskId);
            }
            if (paramGetAllTask.ExecutingUserEmail != null)
            {
                sqlCondition += " AND AssignForEmail = @ExecutingUserEmail";
                param.Add("ExecutingUserEmail", paramGetAllTask.ExecutingUserEmail);
            }

            sql.Append("Drop temporary table if exists TasksGroupTask;");
            sql.Append($"Create temporary table TasksGroupTask Select td.* FROM Task td WHERE {sqlCondition};");
            sql.Append($"SELECT " +
                $"td.* ," +
                $"us.Email," +
                $"us.FirstName," +
                $"us.LastName," +
                $"fa.FileName as FileAvatarName," +
                $"lb.LabelId," +
                $"lb.NameLabel," +
                $"lb.Color " +
                $"FROM TasksGroupTask td " +
                $"LEFT JOIN User us ON us.Email = td.AssignForEmail " +
                $"LEFT JOIN TaskLabel tlb ON tlb.TaskId = td.TaskId " +
                $"LEFT JOIN Label lb ON lb.LabelId = tlb.LabelId " +
                $"LEFT JOIN FileAttachment fa ON fa.AttachmentId = us.UserId " +
                $"WHERE td.GroupTaskId = @GroupTaskQueryId " +
                $"ORDER BY td.SortOrder asc;");

            var result = (List<Task>)_dbConnection.Query<Task, User, Label, Task>(sql.ToString(),
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
                splitOn: "Email,LabelId", commandType: System.Data.CommandType.Text);
            return (List<Task>)taskMap.Values.ToList();
        }

        public int DeleteCustom(ParamDeletGroupTask paramDeletGroupTask)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("GroupTaskQueryId", paramDeletGroupTask.GroupTaskId);

            var result = _dbConnection.Execute("Proc_DeleteGroupTask", param, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public int DeleteMember(string email, Guid groupTaskId, string nameGroupTask) {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Email", email);
            param.Add("GroupTaskId", groupTaskId);

            string sql = "DELETE FROM JoinedGroupTask WHERE UserJoinedEmail = @Email AND GroupTaskReferenceId = @GroupTaskId;";
            var result = _dbConnection.Execute(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public int UpdateRoleMember(string email, Guid groupTaskId, Guid roleId) {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Email", email);
            param.Add("GroupTaskId", groupTaskId);
            param.Add("RoleId", roleId);

            string sql = "UPDATE JoinedGroupTask SET RoleReferenceId = @RoleId WHERE UserJoinedEmail = @Email AND GroupTaskReferenceId = @GroupTaskId;";
            var result = _dbConnection.Execute(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public List<int> GetGeneralCount(Guid groupTaskId)
        { 
            var resultMultiple = _dbConnection.QueryMultiple("Proc_GetGeneralCount",new {
                GroupTaskQueryId = groupTaskId
            }, commandType: System.Data.CommandType.StoredProcedure);

            List<int> listResult = new List<int>();
            var sumOfTask = resultMultiple.Read<int>().FirstOrDefault();
            var countComplete = resultMultiple.Read<int>().FirstOrDefault();
            var countNotComplete = resultMultiple.Read<int>().FirstOrDefault();

            listResult.Add(sumOfTask);
            listResult.Add(countComplete);
            listResult.Add(countNotComplete);

            return listResult;
        }

        public List<object> TaskEachMember(Guid groupTaskId) {
            List<object> listResult = new List<object>();
            var resultMultiple = _dbConnection.QueryMultiple("Proc_GetTaskEachMember", new
            {
                GroupTaskQueryId = groupTaskId
            }, commandType: System.Data.CommandType.StoredProcedure);

            var listTotal = resultMultiple.Read<dynamic>().AsList();
            var listComplete = resultMultiple.Read<dynamic>().AsList();
            var listNotComplete = resultMultiple.Read<dynamic>().AsList();

            listTotal.Sort((before, after) => {
                if (before.TotalTask > after.TotalTask)
                {
                    return -1;
                }
                else if (before.TotalTask < after.TotalTask)
                {
                    return 1;
                }
                else
                    return 0;   
            });

            foreach (var total in listTotal)
            { 
                var totalComplete = listComplete.Find(x => x.Email == total.Email)?.TotalCompleteTask;
                var totalNotComplete = listNotComplete.Find(x => x.Email == total.Email)?.TotalNotCompleteTask;

                listResult.Add(new { 
                    Email = total.Email,
                    UserName = total.UserName,
                    Total = total.TotalTask,
                    TotalComplete = totalComplete,
                    TotalNotComplete = totalNotComplete
                });
            }

            return listResult;
        }

        public List<object> GetStatusExecuteTask(ParamGetStatusExecuteTask paramGetStatusExecuteTask) {
            List<object> listResult = new List<object>();
            var resultMultiple = _dbConnection.QueryMultiple("Proc_GetStatusExecuteTask", new
            {
                GroupTaskQueryId = paramGetStatusExecuteTask.GroupTaskId,
                StartTimeQuery = paramGetStatusExecuteTask.StartTime,
                EndTimeQuery = paramGetStatusExecuteTask.EndTime,
                TimeNowQuery = DateTime.Now
            }, commandType: System.Data.CommandType.StoredProcedure);

            var listTotal = resultMultiple.Read<dynamic>().AsList();
            var listTaskOverProgess = resultMultiple.Read<dynamic>().AsList();
            var listTaskLateDeadline = resultMultiple.Read<dynamic>().AsList();
            var listTaskNeedDone = resultMultiple.Read<dynamic>().AsList();

            listTotal.Sort((before, after) => {
                if (before.TotalTask > after.TotalTask)
                {
                    return -1;
                }
                else if (before.TotalTask < after.TotalTask)
                {
                    return 1;
                }
                else
                    return 0;
            });

            foreach (var total in listTotal)
            {
                var numOfTaskOverProgess = listTaskOverProgess.Find(x => x.Email == total.Email)?.TotalTaskOverProgess;
                var numOfTaskLateDeadline = listTaskLateDeadline.Find(x => x.Email == total.Email)?.TotalTaskLateDeadline;
                var numOfTaskNeedDone = listTaskNeedDone.Find(x => x.Email == total.Email)?.TotalTaskNeedDone;

                listResult.Add(new
                {
                    Email = total.Email,
                    UserName = total.UserName,
                    Total = total.TotalTask,
                    TotalTaskOverProgess = numOfTaskOverProgess != null ? numOfTaskOverProgess : 0,
                    TotalTaskLateDeadline = numOfTaskLateDeadline != null ? numOfTaskLateDeadline : 0,
                    TotalTaskNeedDone = numOfTaskNeedDone != null ? numOfTaskNeedDone : 0,
                    FileAvatarName = total.FileAvatarName
                });
            }

            return listResult;
        }

        public List<object> GetNumOfTaskPersonal(Guid groupTaskId, DateTime startTime, DateTime endTime, string email) {
            List<object> listResult = new List<object>();
            var result = _dbConnection.Query<object>("Proc_GetNumOfTaskPersonal", new
            {
                GroupTaskQueryId = groupTaskId,
                StartTimeQuery = startTime,
                EndTimeQuery = endTime,
                EmailQuery = email
            }, commandType: System.Data.CommandType.StoredProcedure).AsList();

            return result;
        }
    }
}
