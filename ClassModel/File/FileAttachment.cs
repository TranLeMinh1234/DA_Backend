using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.File
{
    public class FileAttachment
    {
        public string FileName { get; set; }
        public string ExtensionOfFile { get; set; }
        public string FilePath { get; set; }
        public Guid? AttachmentId { get; set; }
    }
}
