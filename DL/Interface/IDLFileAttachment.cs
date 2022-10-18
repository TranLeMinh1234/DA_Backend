using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLFileAttachment : IDLBase
    {
        public List<ClassModel.File.FileAttachment> GetAttachFile(Guid taskId);
        public int DeleteMulti(List<string> listFileId);
    }
}
