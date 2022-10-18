using ClassModel.File;
using ClassModel.TaskRelate;
using ClassModel.User;
using Dapper;
using DL.Interface;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public int InsertLabelsTask(Guid taskId, List<string> listLabelId) {
            StringBuilder sql = new StringBuilder();
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskId", taskId);
            for(int i = 0;i < listLabelId.Count;i++)
            {
                sql.Append($"INSERT INTO TaskLabel (TaskId, LabelId) values (@TaskId,@LabelId{i});");
                param.Add($"LabelId{i}",listLabelId.ElementAt(i));
            }
            var result = _dbConnection.Execute(sql.ToString(), param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public List<Label> GetLabelsTask(Guid taskId) {
            string sql = $"SELECT * FROM Label ll INNER JOIN TaskLabel tl ON tl.LabelId = ll.LabelId AND tl.TaskId = @TaskId";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("TaskId", taskId);
            var result = (List<Label>)_dbConnection.Query<Label>(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public int DeleteLabelsTask(Guid taskId, Guid labelId) {
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
            var resultExecutor = _dbConnection.QueryMultiple("Proc_GetCommentsTask",new { TaskId = taskId }, null,null,CommandType.StoredProcedure);
            comments = resultExecutor.Read<Comment>().ToList();
            users = resultExecutor.Read<User>().ToList();
            fileAttachments = resultExecutor.Read<FileAttachment>().ToList();
        }
    }
}
