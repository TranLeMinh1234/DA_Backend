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

        public int InsertLabelsTask(Guid taskId, List<string> listLabelId);

        public List<Label> GetLabelsTask(Guid taskId);
        
        public int DeleteLabelsTask(Guid taskId, Guid labelId);

        public List<Comment> GetCommentsTask(Guid taskId);
    }
}
