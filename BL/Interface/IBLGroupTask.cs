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
        public TemplateGroupTask GetInfoTemplate(Guid groupTaskId, Guid templateReferenceId);
        public List<Task> GetAllTask(ParamGetAllTask paramGetAllTask);
        public int DeleteCustom(ParamDeletGroupTask paramDeletGroupTask);
        public int AddMemebers(ParamAddMember paramAddMember);
        public int DeleteMember(string email, Guid groupTaskId, string nameGroupTask);
    }
}
