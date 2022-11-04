using BL.Interface;
using ClassModel.Notification;
using ClassModel.TaskRelate;
using DL.Interface;
using Newtonsoft.Json;
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
        private WebsocketConnectionManager _websocketConnectionManager;

        public BLComment(WebsocketConnectionManager websocketConnectionManager,IBLNotification iBLNotification, IDLTask iDLTask, IDLComment iDLComment, ContextRequest contextRequest) : base(iDLComment, contextRequest)
        {
            _iDLComment = iDLComment;
            _contextRequest = contextRequest;
            _iDLTask = iDLTask;
            _iBLNotification = iBLNotification;
            _websocketConnectionManager = websocketConnectionManager;
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
                if (task.AssignForEmail != _contextRequest.GetEmailCurrentUser())
                {
                    Notification notificationForUserExecute = new Notification()
                    {
                        CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                        NotificationId = null,
                        GroupTaskRelateId = task.GroupTaskId,
                        NotifyForEmail = task.AssignForEmail,
                        TaskRelateId = taskId,
                        TypeNoti = (int)EnumTypeNotification.CommentedTask,
                        CreatedTime = DateTime.Now
                    };

                    _iBLNotification.Insert(notificationForUserExecute);
                    notificationForUserExecute.Task = task;
                    System.Threading.Tasks.Task.Run(() => _websocketConnectionManager.SendMessageToUser(task.AssignForEmail, JsonConvert.SerializeObject(notificationForUserExecute)));
                }

                if (task.AssignedByEmail != _contextRequest.GetEmailCurrentUser())
                {
                    Notification notificationForUserAssign = new Notification()
                    {
                        CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                        NotificationId = null,
                        GroupTaskRelateId = task.GroupTaskId,
                        NotifyForEmail = task.AssignedByEmail,
                        TaskRelateId = taskId,
                        TypeNoti = (int)EnumTypeNotification.CommentedTask,
                        CreatedTime = DateTime.Now
                    };

                    _iBLNotification.Insert(notificationForUserAssign);

                    notificationForUserAssign.Task = task;
                    System.Threading.Tasks.Task.Run(() => _websocketConnectionManager.SendMessageToUser(task.AssignedByEmail, JsonConvert.SerializeObject(notificationForUserAssign)));
                }
            }

            return comment;
        }
    }


}
