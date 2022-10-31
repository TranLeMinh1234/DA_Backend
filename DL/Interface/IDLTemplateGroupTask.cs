using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLTemplateGroupTask: IDLBase
    {
        public List<TemplateGroupTask> GetAllTemplate(string emailQuery);
        public int DeleteCustom(Guid templateId);

        public Process GetLastestProcess(Guid templateGroupTaskId);

        public bool CheckExistsTaskInProcess(Guid processId);

        public int UpdateSortOrderProcesses(List<ParamUpdateSortOrderProcess> listParam);
    }
}
