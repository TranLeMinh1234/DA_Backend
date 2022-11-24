using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.TaskRelate
{
    public class Label
    {
        [PrimaryKey]
        public Guid? LabelId { get; set; }
        [AddDatabase]
        public string NameLabel { get; set; }
        [AddDatabase]
        public string Color { get; set; }
        [AddDatabase]
        public string CreatedByEmail { get; set; }
        [AddDatabase]
        public string EditByEmail { get; set; }
        [AddDatabase]
        public DateTime? EditTime { get; set; }
        [AddDatabase]
        public DateTime? CreatedTime { get; set; }
        public ClassModel.User.User CreatedBy { get; set; }
        public ClassModel.User.User EditBy { get; set; }
        public string AttachToTaskByEmail { get; set; }
    }
}
