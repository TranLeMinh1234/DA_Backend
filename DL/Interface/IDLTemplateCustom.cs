using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLTemplateCustom : IDLBase
    {
        public int InsertProcessBatch(List<Process> listProcess);
    }
}
