using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLComment: IBLBase
    {
        public Comment InsertCustom(Guid taskId, Comment comment);
    }
}
