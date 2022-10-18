using ClassModel.File;
using ClassModel.TaskRelate;
using ClassModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLTask : IDLBase
    {
        public Task GetLastTask(string Email);
        public List<Task> GetChildTask(Guid taskId);
        public int InsertLabelsTask(Guid taskId, List<string> listLabelId);
        public List<Label> GetLabelsTask(Guid taskId);
        public int DeleteLabelsTask(Guid taskId, Guid labelId);
        public void GetCommentsTask(Guid taskId, out List<Comment> comments, out List<FileAttachment> fileAttachments, out List<User> users);
    }
}
