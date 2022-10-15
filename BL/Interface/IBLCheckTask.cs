using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLCheckTask : IBLBase
    {
        public List<CheckTask> GetCheckTasks(Guid taskId);
    }
}
