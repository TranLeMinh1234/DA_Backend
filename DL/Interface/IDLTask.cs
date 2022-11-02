using ClassModel.File;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using ClassModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLTask : IDLBase
    {
        public Task GetLastTask(string Email, int typeTask, Guid? groupTaskId = null, Guid? processId = null);
        public List<Task> GetChildTask(Guid taskId);
        public int InsertLabelsTask(Guid taskId, List<string> listLabelId);
        public List<Label> GetLabelsTask(Guid taskId);
        public int DeleteLabelsTask(Guid taskId, Guid labelId);
        public void GetCommentsTask(Guid taskId, out List<Comment> comments, out List<FileAttachment> fileAttachments, out List<User> users);
        public int UpdateDescription(Guid taskId, string description);
        public Task GetFullInfo(Guid taskId);
        public List<Task> GetDailyTask(ParamDailyTask paramDailyTask,string email);
        public int DeleteCustom(Guid taskId);
        public int UpdateDeadline(string deadlineUpdate, DateTime? newDeadline, Guid taskId);
        public int InsertRemindDataStore(RemindDataStore remindDataStore);
        public int UpdateTaskProcessBatch(List<ParamUpdateTaskProcessBatch> listParam);
        public int UpdateAssignForUser(Guid taskId, string assignForEmail, string assignedByEmail);
    }
}
