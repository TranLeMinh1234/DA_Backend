using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLGroupTask : IBLBase
    {
        public GroupTask InsertCustom(ParamInserGroupTask paramInserGroupTask);
        public Dictionary<string, object> GetGroupTaskHaveJoined();
        public List<ClassModel.User.User> GetUserJoined(Guid groupTaskId);
        public TemplateCustom GetInfoTemplate(Guid groupTaskId, Guid templateReferenceId);
        public List<Task> GetAllTask(ParamGetAllTask paramGetAllTask);
        public int DeleteCustom(ParamDeletGroupTask paramDeletGroupTask);
        public int AddMemebers(ParamAddMember paramAddMember);
        public int DeleteMember(string email, Guid groupTaskId, string nameGroupTask);
        public int UpdateRoleMember(string email, Guid groupTaskId, Guid roleId, string nameGroupTask);
        public TemplateGroupTask GetInfoTemplateOrigin(Guid templateReferenceId);
        public List<int> GetGeneralCount(Guid groupTaskId);
        public List<object> TaskEachMember(Guid groupTaskId);
    }
}
