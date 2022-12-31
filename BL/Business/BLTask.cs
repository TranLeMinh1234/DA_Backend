using BL.Interface;
using ClassModel;
using ClassModel.Notification;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using DL.Business;
using DL.Interface;
using Newtonsoft.Json;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private WebsocketConnectionManager _websocketConnectionManager;

        public BLTask(WebsocketConnectionManager websocketConnectionManager,IBLNotification iBLNotification ,IDLTask iDLTask, ContextRequest contextRequest, RemindTaskService remindTaskService) : base(iDLTask, contextRequest)
        {
            _iDLTask = iDLTask;
            _contextRequest = contextRequest;
            _iRemindTaskService = remindTaskService;
            _iBLNotification = iBLNotification;
            _websocketConnectionManager = websocketConnectionManager;
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

            newTask = _iDLTask.GetFullInfo((Guid)newTask.TaskId);
            return newTask;
        }

        public List<Task> GetChildTask(Guid taskId)
        {
            var result = _iDLTask.GetChildTask(taskId);
            return result;  
        }

        public int InsertLabelsTask(Guid taskId, List<string> listLabelId) {
            var result = _iDLTask.InsertLabelsTask(taskId, listLabelId, _contextRequest.GetEmailCurrentUser());
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

            if (newIdTask != null && newTask.TypeTask != EnumTypeTask.Personal)
            {
                //nhac nho reload thong tin toan bo grouptask
                Notification notificationReload = new Notification()
                { 
                    TypeNoti = (int)EnumTypeNotification.ReloadGroupTask
                };

                List<string> sentEmails = GetEmailUserJoined((Guid)newTask.GroupTaskId);
                foreach (var email in sentEmails) {
                    if (email != _contextRequest.GetEmailCurrentUser()) {
                        System.Threading.Tasks.Task.Run(() => _websocketConnectionManager.SendMessageToUser(email, JsonConvert.SerializeObject(notificationReload)));
                    }
                    
                }
            }

            return newTask;
        }

        public List<string> GetEmailUserJoined(Guid groupTaskId)
        {
            var result = _iDLTask.GetEmailUserJoined(groupTaskId);
            return result;
        }

        public Task GetFullInfo(Guid taskId) {
            var result = _iDLTask.GetFullInfo(taskId);
            return result;
        }

        public List<Task> GetDailyTask(ParamDailyTask paramDailyTask) {
            var result = _iDLTask.GetDailyTask(paramDailyTask, _contextRequest.GetEmailCurrentUser());
            return result;
        }

        public int DeleteCustom(Guid taskId, bool isNotification = true) {
            Task task = _iDLTask.GetFullInfo(taskId);

            var result = _iDLTask.DeleteCustom(taskId);
            
            if (task.AssignForEmail != _contextRequest.GetEmailCurrentUser() && isNotification)
            {
                Notification notification = new Notification()
                {
                    CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                    NotificationId = null,
                    GroupTaskRelateId = task.GroupTaskId,
                    NotifyForEmail = task.AssignForEmail,
                    TaskRelateId = task.TaskId,
                    TypeNoti = (int)EnumTypeNotification.DeletedTask,
                    CreatedTime = DateTime.Now,
                    TaskName = task.TaskName,
                    ReadStatus = false
                };

                _iBLNotification.Insert(notification);
                System.Threading.Tasks.Task.Run(()=> _websocketConnectionManager.SendMessageToUser(notification.NotifyForEmail,JsonConvert.SerializeObject(notification)));
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
            DateTime? endTime = paramRemindTask.IsRemindEndTime ? (DateTime?)paramRemindTask.EndTime : null,
                    startTime = paramRemindTask.IsRemindStartTime ? (DateTime?)paramRemindTask.StartTime: null;


            if (endTime != null)
            {
                endTime = ((DateTime)endTime).AddSeconds(paramRemindTask.TimeBeforeEndTime * -1);
                if (((DateTime)endTime).Ticks < DateTime.Now.Ticks)
                {
                    serviceResult.Success = false;
                    serviceResult.ErrorCode.Add("InvalidTimeBeforeEndTime");
                }
            }

            if (startTime != null)
            {
                startTime = ((DateTime)startTime).AddSeconds(paramRemindTask.TimeBeforeStartTime * -1);
                if (((DateTime)startTime).Ticks < DateTime.Now.Ticks)
                {
                    serviceResult.Success = false;
                    serviceResult.ErrorCode.Add("InvalidTimeBeforeStartTime");
                }
            }

            if (serviceResult.Success)
            {
                if (endTime != null) {
                    foreach (var email in paramRemindTask.EmailWillSend)
                    {
                        RemindDataStore remindDataStore = new RemindDataStore()
                        {
                            RemindDataId = Guid.NewGuid(),
                            TaskId = paramRemindTask.TaskId,
                            IsUsed = false,
                            EmailRemindedUser = email,
                            TypeRemind = (int)EnumTypeRemind.EndTime
                        };
                        int numberOfInsertedRecord = _iDLTask.InsertRemindDataStore(remindDataStore);
                        if (numberOfInsertedRecord > 0)
                        {
                            await _iRemindTaskService.AddRemindTaskJob((Guid)remindDataStore.RemindDataId, (DateTime)endTime);
                        }
                    }
                }

                if(startTime != null)
                {
                    foreach (var email in paramRemindTask.EmailWillSend)
                    {
                        RemindDataStore remindDataStore = new RemindDataStore()
                        {
                            RemindDataId = Guid.NewGuid(),
                            TaskId = paramRemindTask.TaskId,
                            IsUsed = false,
                            EmailRemindedUser = email,
                            TypeRemind = (int)EnumTypeRemind.StartTime
                        };
                        int numberOfInsertedRecord = _iDLTask.InsertRemindDataStore(remindDataStore);
                        if (numberOfInsertedRecord > 0)
                        {
                            await _iRemindTaskService.AddRemindTaskJob((Guid)remindDataStore.RemindDataId, (DateTime)startTime);
                        }
                    }
                }
            }
            else
            {
                return serviceResult;
            }

            return serviceResult;
        }

        public int UpdateTaskProcessBatch(List<ParamUpdateTaskProcessBatch> listParam) {
            var result = _iDLTask.UpdateTaskProcessBatch(listParam);
            List<string> sentEmails = GetEmailUserJoined((Guid)listParam.ElementAt(0).GroupTaskId);
            if (sentEmails?.Count > 0)
            {
                foreach (var email in sentEmails)
                {
                    if (email != _contextRequest.GetEmailCurrentUser())
                    {
                        Notification notificationReload = new Notification()
                        {
                            TypeNoti = (int)EnumTypeNotification.ReloadGroupTask
                        };
                        System.Threading.Tasks.Task.Run(() => _websocketConnectionManager.SendMessageToUser(email, JsonConvert.SerializeObject(notificationReload)));
                    }
                }
            }

            return result;
        }

        public int UpdateAssignForUser(Guid taskId, Guid groupTaskId, string email) {

            Task task = _iDLTask.GetFullInfo(taskId);
            Notification notification = new Notification()
            {
                CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                NotificationId = null,
                GroupTaskRelateId = groupTaskId,
                NotifyForEmail = email,
                TaskRelateId = taskId,
                TypeNoti = (int)EnumTypeNotification.AssignedTask,
                CreatedTime = DateTime.Now,
                TaskName = task.TaskName,
                ReadStatus = false
            };

             
            var result = _iDLTask.UpdateAssignForUser(taskId, email, _contextRequest.GetEmailCurrentUser());

            if (result > 0)
            {
                _iBLNotification.Insert(notification);
                notification.Task = task;
                System.Threading.Tasks.Task.Run(() => _websocketConnectionManager.SendMessageToUser(notification.NotifyForEmail, JsonConvert.SerializeObject(notification)));
            }
            return result;
        }

        public bool CheckExistsTask(Guid taskId) {
            var result = _iDLTask.CheckExistsTask(taskId);
            return result;
        }

        public int CheckFinished(ParamCheckFinishedTask paramCheckFinishedTask) {
            paramCheckFinishedTask.FinishTime = DateTime.Now;
            var result = _iDLTask.CheckFinished(paramCheckFinishedTask);
            return result;
        }

        public int ConfirmFinishedWork(Guid taskId, int status)
        {
            var result = _iDLTask.ConfirmFinishedWork(taskId, status);
            return result;
        }
    }
}
