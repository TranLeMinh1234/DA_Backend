using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.TaskRelate
{
    public class CommentDapper
    {
        [PrimaryKey]
        public Guid? CommentId { get; set; }
        [AddDatabase]
        public string Content { get; set; }
        [AddDatabase]
        public string CreatedByEmail { get; set; }
        [AddDatabase]
        public DateTime? CreatedTime { get; set; }
        [AddDatabase]
        public Guid? AttachmentId { get; set; }
    }
}
