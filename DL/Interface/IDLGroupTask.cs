using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using ClassModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLGroupTask : IDLBase
    {
        public int InsertBatchJoinedGroupTask(List<JoinedGroupTask> joinedGroupTasks);
        public Dictionary<string, object> GetGroupTaskHaveJoined(string email);
        public List<ClassModel.User.User> GetUserJoined(Guid groupTaskId);
        public TemplateCustom GetInfoTemplate(Guid groupTaskId, Guid templateReferenceId);
        public List<Task> GetAllTask(ParamGetAllTask paramGetAllTask);
        public int DeleteCustom(ParamDeletGroupTask paramDeletGroupTask);
        public int DeleteMember(string email, Guid groupTaskId, string nameGroupTask);
        public int UpdateRoleMember(string email, Guid groupTaskId, Guid roleId);
        public TemplateGroupTask GetInfoTemplateOrigin(Guid templateReferenceId);
        public List<int> GetGeneralCount(Guid groupTaskId);
        public List<object> TaskEachMember(Guid groupTaskId);
        public List<object> GetStatusExecuteTask(ParamGetStatusExecuteTask paramGetStatusExecuteTask);
        public List<object> GetNumOfTaskPersonal(Guid groupTaskId, DateTime startTime, DateTime endTime, string email);
    }
}
