using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLFileAttachment : IBLBase
    {
        public List<ClassModel.File.FileAttachment> GetAttachFile(Guid taskId);
        public int DeleteMulti(List<string> listFileId);
    }
}
