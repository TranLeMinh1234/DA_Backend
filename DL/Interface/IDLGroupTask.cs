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
        public TemplateGroupTask GetInfoTemplate(Guid groupTaskId, Guid templateReferenceId);
        public List<Task> GetAllTask(ParamGetAllTask paramGetAllTask);
        public int DeleteCustom(ParamDeletGroupTask paramDeletGroupTask);
    }
}
