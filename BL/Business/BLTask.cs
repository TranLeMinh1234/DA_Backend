using BL.Interface;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;
using static ClassModel.Enumeration;

namespace BL.Business
{
    public class BLTask : BLBase, IBLTask
    {
        private IDLTask _iDLTask;
        private ContextRequest _contextRequest;

        public BLTask(IDLTask iDLTask, ContextRequest contextRequest) : base(iDLTask, contextRequest)
        {
            _iDLTask = iDLTask;
            _contextRequest = contextRequest;
        }

        public Task InsertChildTask(Task newTask)
        {
            newTask.CreatedByEmail = _contextRequest.GetEmailCurrentUser();
            newTask.CreatedTime = DateTime.Now;
            newTask.AssignForEmail = _contextRequest.GetEmailCurrentUser();

            Task taskLast = _iDLTask.GetLastTask(_contextRequest.GetEmailCurrentUser(), (int)newTask.TypeTask);
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
                        comment.lstFileAttachment.Add(file);
                    }
                }

                foreach (var user in users)
                {
                    if (user.Email == comment.CreatedByEmail)
                    {
                        comment.user = user;
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

            Task lastestTask = _iDLTask.GetLastTask(_contextRequest.GetEmailCurrentUser(), (int)newTask.TypeTask);
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
            var result = _iDLTask.DeleteCustom(taskId);
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
    }
}
