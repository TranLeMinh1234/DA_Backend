using BL.Interface;
using ClassModel.TaskRelate;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Business
{
    public class BLTask : BLBase, IBLTask
    {
        private IDLTask _iDLTask;
        private ContextRequest _contextRequest;

        public BLTask(IDLTask iDLTask, ContextRequest contextRequest) : base(iDLTask, contextRequest)
        {
            _iDLTask = iDLTask;
            _contextRequest = contextRequest;
        }

        public Task InsertChildTask(Task newTask)
        {
            newTask.CreatedByEmail = _contextRequest.GetEmailCurrentUser();
            newTask.CreatedTime = DateTime.Now;
            newTask.AssignForEmail = _contextRequest.GetEmailCurrentUser();

            Task taskLast = _iDLTask.GetLastTask(_contextRequest.GetEmailCurrentUser());
            if (taskLast != null)
                newTask.SortOrder = taskLast.SortOrder + 1;
            else
                newTask.SortOrder = 1;

            _iDLTask.Insert(newTask);
            return newTask;
        }

        public List<Task> GetChildTask(Guid taskId)
        {
            var result = _iDLTask.GetChildTask(taskId);
            return result;
        }
    }
}
