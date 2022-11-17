using BL.Interface;
using ClassModel.Notification;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using ClassModel.User;
using DL.Interface;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static ClassModel.Enumeration;

namespace BL.Business
{
    public class BLGroupTask : BLBase, IBLGroupTask
    {
        private IDLGroupTask _iDLGroupTask;
        private ContextRequest _contextRequest;
        private IBLNotification _iBlNotification;
        private WebsocketConnectionManager _websocketConnectionManager;
        private IBLTask _iBLTask;

        public BLGroupTask(IBLTask iBLTask, WebsocketConnectionManager websocketConnectionManager,IBLNotification iBLNotification,IDLGroupTask iDLGroupTask, ContextRequest contextRequest) : base(iDLGroupTask, contextRequest)
        {
            _iDLGroupTask = iDLGroupTask;
            _contextRequest = contextRequest;
            _iBlNotification = iBLNotification;
            _websocketConnectionManager = websocketConnectionManager;
            _iBLTask = iBLTask;
        }

        public GroupTask InsertCustom(ParamInserGroupTask paramInserGroupTask) {
            GroupTask groupTask = paramInserGroupTask.GroupTask;
            groupTask.CreatedByEmail = _contextRequest.GetEmailCurrentUser();
            groupTask.CreatedTime = DateTime.Now;

            Guid?  newIdGroupTask = _iDLGroupTask.Insert(groupTask);
            if (newIdGroupTask != null && groupTask.TypeGroupTask == (int)EnumTypeGroupTask.Group || newIdGroupTask != null && groupTask.TypeGroupTask == (int)EnumTypeGroupTask.Personal)
            {
                List<JoinedGroupTask> listJoinedGroupTask = new List<JoinedGroupTask>();
                List<Notification> listNotification = new List<Notification>();
                foreach (ClassModel.User.User userJoin in paramInserGroupTask.ListUser)
                {
                    JoinedGroupTask joinedGroupTask = new JoinedGroupTask() {
                        JoinId = null,
                        InvitedByEmail = _contextRequest.GetEmailCurrentUser(),
                        UserJoinedEmail = userJoin.Email,
                        JoinedTime = DateTime.Now,
                        GroupTaskReferenceId = newIdGroupTask,
                        RoleReferenceId = userJoin.Role.RoleId
                    };

                    if (userJoin.Email != _contextRequest.GetEmailCurrentUser())
                    {
                        Notification notification = new Notification()
                        {
                            CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                            NotificationId = null,
                            GroupTaskRelateId = newIdGroupTask,
                            NotifyForEmail = userJoin.Email,
                            TaskRelateId = null,
                            TypeNoti = (int)EnumTypeNotification.AddUserGroupTask,
                            RoleRelateId = userJoin.Role.RoleId,
                            CreatedTime = DateTime.Now,
                            ReadStatus = false
                        };
                        listNotification.Add(notification);
                    }

                    listJoinedGroupTask.Add(joinedGroupTask);
                }

                _iDLGroupTask.InsertBatchJoinedGroupTask(listJoinedGroupTask);
                if (listNotification.Count > 0)
                {
                    _iBlNotification.InsertBatch(listNotification);
                }

                foreach (var notification in listNotification)
                {
                    System.Threading.Tasks.Task.Run(() => _websocketConnectionManager.SendMessageToUser(notification.NotifyForEmail, JsonConvert.SerializeObject(notification)));
                }
            }

            return groupTask;
        }

        public Dictionary<string, object> GetGroupTaskHaveJoined() {
            var result = _iDLGroupTask.GetGroupTaskHaveJoined(_contextRequest.GetEmailCurrentUser());
            return result;
        }

        public List<ClassModel.User.User> GetUserJoined(Guid groupTaskId) {
            var result = _iDLGroupTask.GetUserJoined(groupTaskId);
            return result;
        }

        public TemplateGroupTask GetInfoTemplate(Guid groupTaskId, Guid templateReferenceId) {
            var result = _iDLGroupTask.GetInfoTemplate(groupTaskId, templateReferenceId);
            return result;
        }

        public List<Task> GetAllTask(ParamGetAllTask paramGetAllTask) {
            var result = _iDLGroupTask.GetAllTask(paramGetAllTask);
            return result;
        }

        public int DeleteCustom(ParamDeletGroupTask paramDeletGroupTask) {

            List<ClassModel.User.User> listUserJoined = _iDLGroupTask.GetUserJoined((Guid)paramDeletGroupTask.GroupTaskId);
            List<Notification> listNotification = new List<Notification>();
            List<Guid> listTaskId = paramDeletGroupTask.ListTaskId;
            GroupTask groupTask = _iDLGroupTask.GetById<GroupTask>((Guid)paramDeletGroupTask.GroupTaskId);
            if (listUserJoined != null && groupTask != null) {
                foreach (var user in listUserJoined) {
                    if (user.Email != _contextRequest.GetEmailCurrentUser())
                    {
                        Notification notification = new Notification()
                        {
                            CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                            NotificationId = null,
                            GroupTaskRelateId = paramDeletGroupTask.GroupTaskId,
                            NotifyForEmail = user.Email,
                            TaskRelateId = null,
                            TypeNoti = (int)EnumTypeNotification.DeleteGroupTask,
                            RoleRelateId = user.Role.RoleId,
                            CreatedTime = DateTime.Now,
                            ReadStatus = false,
                            NameGroupTask = groupTask.NameGroupTask
                        };
                        listNotification.Add(notification);
                    }
                }
            }

            var result = _iDLGroupTask.DeleteCustom(paramDeletGroupTask);

            if (result > 0) {
                foreach (var taskId in listTaskId)
                {
                    _iBLTask.DeleteCustom(taskId, false);
                }

                foreach (var notification in listNotification)
                { 
                    System.Threading.Tasks.Task.Run(() => _websocketConnectionManager.SendMessageToUser(notification.NotifyForEmail, JsonConvert.SerializeObject(notification)));
                }
            }
            return result;
        }

        public int AddMemebers(ParamAddMember paramAddMember) {
            List<ClassModel.User.User> listUser = paramAddMember.ListUser;
            List<JoinedGroupTask> listJoinedGroupTask = new List<JoinedGroupTask>();
            List<Notification> listNotification = new List<Notification>();
            foreach (var member in listUser)
            {
                JoinedGroupTask joinedGroupTask = new JoinedGroupTask()
                {
                    JoinId = null,
                    JoinedTime = DateTime.Now,
                    InvitedByEmail = _contextRequest.GetEmailCurrentUser(),
                    GroupTaskReferenceId = paramAddMember.GroupTaskId,
                    UserJoinedEmail = member.Email,
                    RoleReferenceId = member.Role.RoleId
                };

                if (member.Email != _contextRequest.GetEmailCurrentUser())
                {
                    Notification notification = new Notification()
                    {
                        CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                        NotificationId = null,
                        GroupTaskRelateId = paramAddMember.GroupTaskId,
                        NotifyForEmail = member.Email,
                        TaskRelateId = null,
                        TypeNoti = (int)EnumTypeNotification.AddUserGroupTask,
                        RoleRelateId = member.Role.RoleId,
                        CreatedTime = DateTime.Now,
                        ReadStatus = false
                    };
                    listNotification.Add(notification);
                }

                listJoinedGroupTask.Add(joinedGroupTask);
            }

            var result = _iDLGroupTask.InsertBatchJoinedGroupTask(listJoinedGroupTask);
            if (listNotification.Count > 0)
            {
                _iBlNotification.InsertBatch(listNotification);
                foreach (var notification in listNotification)
                {
                    System.Threading.Tasks.Task.Run(() => _websocketConnectionManager.SendMessageToUser(notification.NotifyForEmail, JsonConvert.SerializeObject(notification)));
                }
            }

            return result;
        }

        public int DeleteMember(string email, Guid groupTaskId, string nameGroupTask) {
            var result = _iDLGroupTask.DeleteMember(email,groupTaskId,nameGroupTask);
            if (result > 0)
            {
                Notification notification = new Notification()
                {
                    CreatedByEmail = _contextRequest.GetEmailCurrentUser(),
                    NotificationId = null,
                    GroupTaskRelateId = groupTaskId,
                    NotifyForEmail = email,
                    TaskRelateId = null,
                    TypeNoti = (int)EnumTypeNotification.DeleteUserFromGroupTask,
                    RoleRelateId = null,
                    CreatedTime = DateTime.Now,
                    ReadStatus = false,
                    NameGroupTask = nameGroupTask
                };

                _iBlNotification.Insert(notification);
                System.Threading.Tasks.Task.Run(() => _websocketConnectionManager.SendMessageToUser(notification.NotifyForEmail, JsonConvert.SerializeObject(notification)));
            }

            return result;
        }
    }
}
