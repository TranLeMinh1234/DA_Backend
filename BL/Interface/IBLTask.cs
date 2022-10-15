using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLTask : IBLBase
    {
        public Task InsertChildTask(Task newTask);

        public List<Task> GetChildTask(Guid taskId);
    }
}
