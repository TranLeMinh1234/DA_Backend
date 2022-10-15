using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLCheckTask : IDLBase
    {
        public List<CheckTask> GetCheckTasks(Guid taskId);
    }
}
