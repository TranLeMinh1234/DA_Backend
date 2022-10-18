using BL.Interface;
using ClassModel.TaskRelate;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

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

            Task taskLast = _iDLTask.GetLastTask(_contextRequest.GetEmailCurrentUser());
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
    }
}
