using BL.Interface;
using ClassModel;
using ClassModel.Notification;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using DL.Interface;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static ClassModel.Enumeration;

namespace BL.Business
{
    public class BLTask : BLBase, IBLTask
    {
        private IDLTask _iDLTask;
        private IBLNotification _iBLNotification;
        private ContextRequest _contextRequest;
        private RemindTaskService _iRemindTaskService;

        public BLTask(IBLNotification iBLNotification ,IDLTask iDLTask, ContextRequest contextRequest, RemindTaskService remindTaskService) : base(iDLTask, contextRequest)
        {
            _iDLTask = iDLTask;
            _contextRequest = contextRequest;
            _iRemindTaskService = remindTaskService;
            _iBLNotification = iBLNotification;
        }

        public Task InsertChildTask(Task newTask)
        {
            newTask.CreatedByEmail = _contextRequest.GetEmailCurrentUser();
            newTask.CreatedTime = DateTime.Now;
            newTask.AssignForEmail = _contextRequest.GetEmailCurrentUser();

            Task taskLast = _iDLTask.GetLastTask(_contextRequest.GetEmailCurrentUser(), (int)newTask.TypeTask, newTask.GroupTaskId, newTask.ProcessId);
            if (taskLast != null)
                newTask.SortOrder = taskLast.SortOrder + 1;
            else
                newTask.SortOrder = 1;

            _iDLTask.Insert(newTask);
            return newTask;
        }

        public List<Task> GetChildTask(Guid taskId)
        {
            var result = _iDLTask.GetChildTask(taskId);
            return result;
        }

        public int InsertLabelsTask(Guid taskId, List<string> listLabelId) {
            var result = _iDLTask.InsertLabelsTask(taskId, listLabelId);
            return result;
        }

        public List<Label> GetLabelsTask(Guid taskId) {
            var result = _iDLTask.GetLabelsTask(taskId);
            return result;
        }

        public int DeleteLabelsTask(Guid taskId, Guid labelId) {
            var result = _iDLTask.DeleteLabelsTask(taskId, labelId);
            return result;
        }

        public List<Comment> GetCommentsTask(Guid taskId)
        {
            List<Comment> comments = null;
            List<ClassModel.File.FileAttachment> fileAttachments = null;
            List<ClassModel.User.User> users = null;
            _iDLTask.GetCommentsTask(taskId,out comments,out fileAttachments, out users);

            foreach (var comment in comments)
            {
                foreach (var file in fileAttachments)
                {
                    if (comment.CommentId == file.AttachmentId)
                    {
                        comment.LstFileAttachment.Add(file);
                    }
                }

                foreach (var user in users)
                {
                    if (user.Email == comment.CreatedByEmail)
                    {
                        comment.User = user;
                    }
                }
            }

            return comments;
        }

        public int UpdateDescription(Dictionary<string, string> paramUpdate)
        {
            string description = String.Empty;
            string taskIdString = String.Empty;
            if (paramUpdate.TryGetValue("description", out description) && paramUpdate.TryGetValue("taskId", out taskIdString))
            {
                var taskId = Guid.Parse(taskIdString);
                var result = _iDLTask.UpdateDescription(taskId,description);
                return result;
            }

            return 0;
        }

        public Task InsertCustom(Task newTask)
        {
            newTask.CreatedTime = DateTime.Now;
            newTask.CreatedByEmail = _contextRequest.GetEmailCurrentUser();
            newTask.AssignForEmail = _contextRequest.GetEmailCurrentUser();
            newTask.AssignedByEmail = _contextRequest.GetEmailCurrentUser();

            Task lastestTask = _iDLTask.GetLastTask(_contextRequest.GetEmailCurrentUser(), (int)newTask.TypeTask, newTask.GroupTaskId, newTask.ProcessId);

            if(lastestTask != null)
                newTask.SortOrder = lastestTask.SortOrder + 1;
            else
                newTask.SortOrder = 1;

            var newIdTask = _iDLTask.Insert(newTask);
            newTask.TaskId = newIdTask;

            return newTask;
        }

        public Task GetFullInfo(Guid taskId) {
            var result = _iDLTask.GetFullInfo(taskId);
            return result;
        }

        public List<Task> GetDailyTask(ParamDailyTask paramDailyTask) {
            var result = _iDLTask.GetDailyTask(paramDailyTask, _contextRequest.GetEmailCurrentUser());
            return result;
        }

        public int DeleteCustom(Guid taskId) {
            Task task = _iDLTask.GetFullInfo(taskId);

            var result = _iDLTask.DeleteCustom(taskId);
            
            if (task.AssignedByEmail != _contextRequest.GetEmailCurrentUser())
            {
                Notification notification = new Notification()
                {
                    CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                    NotificationId = null,
                    GroupTaskRelateId = task.GroupTaskId,
                    NotifyForEmail = task.AssignForEmail,
                    TaskRelateId = task.TaskId,
                    TypeNoti = (int)EnumTypeNotification.DeletedTask,
                    CreatedTime = DateTime.Now
                };

                _iBLNotification.Insert(notification);
            }

            return result;
        }

        public int UpdateDeadline(int typeDeadline, DateTime? newDeadline, Guid taskId) {
            string deadlineUpdate = string.Empty;
            if (typeDeadline == (int)EnumTypeDeadline.Start)
            {
                deadlineUpdate = "StartTime";
            }
            else
            {
                deadlineUpdate = "EndTime";
            }
            var result = _iDLTask.UpdateDeadline(deadlineUpdate, newDeadline, taskId);

            return result;
        }

        public async System.Threading.Tasks.Task<ServiceResult> RemindTask(ParamRemindTask paramRemindTask) {
            ServiceResult serviceResult = new ServiceResult();

            if (paramRemindTask.IsRemindEndTime)
            {
                DateTime endTime = (DateTime)paramRemindTask.EndTime;
                endTime = endTime.AddSeconds(paramRemindTask.TimeBeforeEndTime*-1);
                if (endTime.Ticks < DateTime.Now.Ticks)
                {
                    serviceResult.Success = false;
                    serviceResult.ErrorCode.Add("InvalidTimeBeforeEndTime");
                }
                else
                {
                    foreach (var email in paramRemindTask.EmailWillSend)
                    {
                        RemindDataStore remindDataStore = new RemindDataStore() {
                            RemindDataId = Guid.NewGuid(),
                            TaskId = paramRemindTask.TaskId,
                            IsUsed = false,
                            EmailRemindedUser = "tlminh10300@gmail.com",
                            TypeRemind = (int)EnumTypeRemind.EndTime
                        };
                        int numberOfInsertedRecord = _iDLTask.InsertRemindDataStore(remindDataStore);
                        if (numberOfInsertedRecord > 0)
                        {
                            await _iRemindTaskService.AddRemindTaskJob((Guid)remindDataStore.RemindDataId, endTime);
                        }
                    }
                }
            }

            if (paramRemindTask.IsRemindStartTime)
            {
                DateTime startTime = (DateTime)paramRemindTask.StartTime;
                startTime = startTime.AddSeconds(paramRemindTask.TimeBeforeStartTime * -1);
                if (startTime.Ticks < DateTime.Now.Ticks)
                {
                    serviceResult.Success = false;
                    serviceResult.ErrorCode.Add("InvalidTimeBeforeStartTime");
                }
                else
                {
                    foreach (var email in paramRemindTask.EmailWillSend)
                    {
                        RemindDataStore remindDataStore = new RemindDataStore()
                        {
                            RemindDataId = Guid.NewGuid(),
                            TaskId = paramRemindTask.TaskId,
                            IsUsed = false,
                            EmailRemindedUser = "tlminh10300@gmail.com",
                            TypeRemind = (int)EnumTypeRemind.StartTime
                        };
                        int numberOfInsertedRecord = _iDLTask.InsertRemindDataStore(remindDataStore);
                        if (numberOfInsertedRecord > 0)
                        {
                            await _iRemindTaskService.AddRemindTaskJob((Guid)remindDataStore.RemindDataId, startTime);
                        }
                    }
                }
            }


            return serviceResult;
        }

        public int UpdateTaskProcessBatch(List<ParamUpdateTaskProcessBatch> listParam) {
            var result = _iDLTask.UpdateTaskProcessBatch(listParam);
            return result;
        }

        public int UpdateAssignForUser(Guid taskId, Guid groupTaskId, string email) {
            Notification notification = new Notification()
            {
                CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                NotificationId = null,
                GroupTaskRelateId = groupTaskId,
                NotifyForEmail = email,
                TaskRelateId = taskId,
                TypeNoti = (int)EnumTypeNotification.AssignedTask,
                CreatedTime = DateTime.Now
            };

             
            var result = _iDLTask.UpdateAssignForUser(taskId, email, _contextRequest.GetEmailCurrentUser());

            if (result > 0)
            {
                _iBLNotification.Insert(notification);
            }
            return result;
        }
        
    }
}
