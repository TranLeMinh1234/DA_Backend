using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLTemplateCustom: IBLBase
    {
        public TemplateCustom InsertCustom(TemplateGroupTask templateGroupTask);
        public ClassModel.TaskRelate.Process InsertProcess(ClassModel.TaskRelate.Process process);
        public int UpdateSortOrderProcesses(List<ParamUpdateSortOrderProcess> listParam);
        public int InsertProcessBatch(List<Process> listProcess);
    }
}
