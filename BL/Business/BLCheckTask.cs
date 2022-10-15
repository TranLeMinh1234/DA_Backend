using BL.Interface;
using ClassModel.TaskRelate;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Business
{
    public class BLCheckTask : BLBase, IBLCheckTask
    {
        protected IDLCheckTask _iDLCheckTask;
        protected ContextRequest _contextRequest;

        public BLCheckTask(IDLCheckTask iDLCheckTask, ContextRequest contextRequest): base(iDLCheckTask,contextRequest)
        {
            _iDLCheckTask = iDLCheckTask;
            _contextRequest = contextRequest;
        }

        public List<CheckTask> GetCheckTasks(Guid taskId) {
            var result = _iDLCheckTask.GetCheckTasks(taskId);
            return result;
        }
    }
}
