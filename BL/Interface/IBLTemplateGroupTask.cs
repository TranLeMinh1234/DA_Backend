using ClassModel;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BL.Interface
{
    public interface IBLTemplateGroupTask : IBLBase
    {
        public TemplateGroupTask InsertCustom(TemplateGroupTask templateGroupTask);

        public List<TemplateGroupTask> GetAllTemplate();

        public int DeleteCustom(Guid templateId);

        public Guid? UpdateProcess(ClassModel.TaskRelate.Process process);

        public ClassModel.TaskRelate.Process InsertProcess(ClassModel.TaskRelate.Process process);

        public ServiceResult DeleteProcess(Guid processId, Guid columnSettingId);

        public int UpdateSortOrderProcesses(List<ParamUpdateSortOrderProcess> listParam);
    }
}
