using BL.Interface;
using ClassModel.Notification;
using ClassModel.TaskRelate;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using static ClassModel.Enumeration;

namespace BL.Business
{
    public class BLComment : BLBase, IBLComment
    {
        private IDLComment _iDLComment;
        private ContextRequest _contextRequest;
        private IDLTask _iDLTask;
        private IBLNotification _iBLNotification;

        public BLComment(IBLNotification iBLNotification, IDLTask iDLTask, IDLComment iDLComment, ContextRequest contextRequest) : base(iDLComment, contextRequest)
        {
            _iDLComment = iDLComment;
            _contextRequest = contextRequest;
            _iDLTask = iDLTask;
            _iBLNotification = iBLNotification;
        }

        public Comment InsertCustom(Guid taskId, Comment comment)
        {
            comment.CreatedTime = DateTime.Now;
            comment.AttachmentId = taskId;
            comment.CreatedByEmail = _contextRequest.GetEmailCurrentUser();

            var result = _iDLComment.Insert(comment);

            if (result != null)
            {
                Task task = _iDLTask.GetFullInfo(taskId);
                Notification notification = new Notification()
                {
                    CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                    NotificationId = null,
                    GroupTaskRelateId = task.GroupTaskId,
                    NotifyForEmail = task.AssignForEmail,
                    TaskRelateId = taskId,
                    TypeNoti = (int)EnumTypeNotification.CommentedTask,
                    CreatedTime = DateTime.Now
                };

                _iBLNotification.Insert(notification);
            }

            return comment;
        }
    }


}
