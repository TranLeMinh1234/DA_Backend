using ClassModel.TaskRelate;
using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL.Business
{
    public class DLTemplateCustom : DLBase, IDLTemplateCustom
    {
        public int InsertProcessBatch(List<Process> listProcess) {
            Dictionary<string, object> param = new Dictionary<string, object>();

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= listProcess.Count; i++) {
                sb.Append($"INSERT INTO Process (ProcessId,ProcessName,ColumnSettingReferenceId,SortOrder,CreatedByEmail,CreatedTime,TemplateGroupTaskReferenceId,Description)" + 
                    $" VALUES(@ProcessId{i},@ProcessName{i},@ColumnSettingReferenceId{i},@SortOrder{i},@CreatedByEmail{i},@CreatedTime{i},@TemplateGroupTaskReferenceId{i},@Description{i});");

                param.Add($"ProcessId{i}",Guid.NewGuid());
                param.Add($"ProcessName{i}", listProcess.ElementAt(i-1).ProcessName);
                param.Add($"ColumnSettingReferenceId{i}", listProcess.ElementAt(i - 1).ColumnSettingReferenceId);
                param.Add($"SortOrder{i}", listProcess.ElementAt(i - 1).SortOrder);
                param.Add($"CreatedByEmail{i}", listProcess.ElementAt(i - 1).CreatedByEmail);
                param.Add($"CreatedTime{i}", listProcess.ElementAt(i - 1).CreatedTime);
                param.Add($"TemplateGroupTaskReferenceId{i}", listProcess.ElementAt(i - 1).TemplateGroupTaskReferenceId);
                param.Add($"Description{i}", listProcess.ElementAt(i - 1).Description);
            }

            var result = _dbConnection.Execute(sb.ToString(),param,commandType: System.Data.CommandType.Text);
            return result;
        }
    }
}
