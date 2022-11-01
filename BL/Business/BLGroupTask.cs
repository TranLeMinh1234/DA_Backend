using BL.Interface;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using ClassModel.User;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Business
{
    public class BLGroupTask : BLBase, IBLGroupTask
    {
        private IDLGroupTask _iDLGroupTask;
        private ContextRequest _contextRequest;

        public BLGroupTask(IDLGroupTask iDLGroupTask, ContextRequest contextRequest) : base(iDLGroupTask, contextRequest)
        {
            _iDLGroupTask = iDLGroupTask;
            _contextRequest = contextRequest;
        }

        public GroupTask InsertCustom(ParamInserGroupTask paramInserGroupTask) {
            GroupTask groupTask = paramInserGroupTask.GroupTask;
            groupTask.CreatedByEmail = _contextRequest.GetEmailCurrentUser();
            groupTask.CreatedTime = DateTime.Now;

            Guid?  newIdGroupTask = _iDLGroupTask.Insert(groupTask);
            if (newIdGroupTask != null)
            {
                List<JoinedGroupTask> listJoinedGroupTask = new List<JoinedGroupTask>();
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
                    listJoinedGroupTask.Add(joinedGroupTask);
                }

                _iDLGroupTask.InsertBatchJoinedGroupTask(listJoinedGroupTask);
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
    }
}
