using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.File
{
    public class FileAttachment
    {
        [PrimaryKey]
        public Guid FileId { get; set; }
        [AddDatabase]
        public string FileName { get; set; }
        [AddDatabase]
        public string ExtensionOfFile { get; set; }
        [AddDatabase]
        public string FilePath { get; set; }
        [AddDatabase]
        public Guid? AttachmentId { get; set; }
        [AddDatabase]
        public int TypeAttachment { get; set; }
    }
}
