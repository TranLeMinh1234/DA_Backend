using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ClassModel.Enumeration;

namespace DL.DLFile
{
    public class DLFileAttachment : DLBase,IDLFileAttachment
    {
        public List<ClassModel.File.FileAttachment> GetAttachFile(Guid taskId) {
            string sql = $"SELECT * FROM FileAttachment WHERE AttachmentId = @AttachmentId AND TypeAttachment = @TypeAttachment";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("AttachmentId", taskId);
            param.Add("TypeAttachment", EnumAttachment.AttachTask);
            var result = (List<ClassModel.File.FileAttachment>)_dbConnection.Query<ClassModel.File.FileAttachment>(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public int DeleteMulti(List<string> listFileId) {
            string sqlFileIdParam = "";
            Dictionary<string, object> param = new Dictionary<string, object>();
            for (int i = 0; i < listFileId.Count; i++)
            {
                sqlFileIdParam += $"@FileId{i}, ";
                param.Add($"@FileId{i}", listFileId.ElementAt(i));
            }

            sqlFileIdParam = sqlFileIdParam.Remove(sqlFileIdParam.Length - 2);

            string sql = $"DELETE FROM FileAttachment Where FileId In ({sqlFileIdParam});";
            var result = _dbConnection.Execute(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }
    }
}
