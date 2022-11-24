using ClassModel;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLTask : IBLBase
    {
        public Task InsertChildTask(Task newTask);

        public List<Task> GetChildTask(Guid taskId);

        public int InsertLabelsTask(Guid taskId, List<string> listLabelId);

        public List<Label> GetLabelsTask(Guid taskId);

        public int DeleteLabelsTask(Guid taskId, Guid labelId);

        public List<Comment> GetCommentsTask(Guid taskId);

        public int UpdateDescription(Dictionary<string, string> paramUpdate);

        public Task InsertCustom(Task newTask);

        public Task GetFullInfo(Guid taskId);

        public List<Task> GetDailyTask(ParamDailyTask paramDailyTask);

        public int DeleteCustom(Guid taskId, bool isNotification = true);

        public int UpdateDeadline(int typeDeadline, DateTime? newDeadline, Guid taskId);

        public System.Threading.Tasks.Task<ServiceResult> RemindTask(ParamRemindTask paramRemindTask);

        public int UpdateTaskProcessBatch(List<ParamUpdateTaskProcessBatch> listParam);

        public int UpdateAssignForUser(Guid taskId,Guid groupTaskId,string email);

        public bool CheckExistsTask(Guid taskId);

        public List<string> GetEmailUserJoined(Guid groupTaskId);

        public int CheckFinished(ParamCheckFinishedTask paramCheckFinishedTask);

        public int ConfirmFinishedWork(Guid taskId, int status);
    }
}
