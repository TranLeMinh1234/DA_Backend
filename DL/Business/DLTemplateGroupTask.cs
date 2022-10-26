using ClassModel.File;
using ClassModel.TaskRelate;
using ClassModel.User;
using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DL.Business
{
    public class DLTemplateGroupTask : DLBase, IDLTemplateGroupTask
    {
        public List<TemplateGroupTask> GetAllTemplate(string emailQuery) {
            Dictionary<string, TemplateGroupTask> mapResult = new Dictionary<string, TemplateGroupTask>();
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("EmailQuery", emailQuery);
            var result = _dbConnection.Query<TemplateGroupTask, Process, ColumnSetting,User, TemplateGroupTask>("Proc_GetAllTemplateGroupTask",
                (templateGroupTask, process, columnSetting, user) => {
                    if (mapResult.ContainsKey(templateGroupTask.TemplateGroupTaskId.ToString()))
                    {
                        if (mapResult.TryGetValue(templateGroupTask.TemplateGroupTaskId.ToString(), out templateGroupTask))
                        {
                            if (process != null)
                            {
                                process.ColumnSetting = columnSetting;
                                templateGroupTask.CreatedBy = user;
                                templateGroupTask.ListProcess.Add(process);
                            }
                        }
                    }
                    else
                    {
                        if (process != null)
                        {
                            process.ColumnSetting = columnSetting;
                            templateGroupTask.ListProcess.Add(process);
                        }
                        templateGroupTask.CreatedBy = user;
                        mapResult.Add(templateGroupTask.TemplateGroupTaskId.ToString(), templateGroupTask);
                    }

                    return templateGroupTask;
                },
                param,
                splitOn: "ProcessId,ColumnSettingId,FirstName",
                commandType: CommandType.StoredProcedure);
            return mapResult.Values.AsList();
        }

        public int DeleteCustom(Guid templateId) {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TemplateQueryId", templateId);
            var result = _dbConnection.Execute("Proc_DeleteTemplateCustom", param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Process GetLastestProcess(Guid templateGroupTaskId) {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TemplateGroupTaskId", templateGroupTaskId);
            string sql = $"SELECT * FROM Process WHERE TemplateGroupTaskReferenceId = @TemplateGroupTaskId ORDER BY SortOrder desc LIMIT 0,1;";
            var result = _dbConnection.Query<Process>(sql, param, commandType: CommandType.Text).FirstOrDefault();
            return result;
        }

        public bool CheckExistsTaskInProcess(Guid processId) {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("ProcessId", processId);
            string sql = "SELECT EXISTS(SELECT * FROM Task WHERE ProcessId = @ProcessId LIMIT 0,1);";
            var result = _dbConnection.ExecuteScalar<bool>(sql, param, commandType: CommandType.Text);
            return result;
        }
    }
}
