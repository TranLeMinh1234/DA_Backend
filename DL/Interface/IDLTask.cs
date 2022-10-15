using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLTask : IDLBase
    {
        public Task GetLastTask(string Email);
        public List<Task> GetChildTask(Guid taskId);
    }
}
