using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.TaskRelate
{
    public class TemplateGroupTask
    {
        [PrimaryKey]
        public Guid? TemplateGroupTaskId { get; set; }
        [AddDatabase]
        public string NameTemplateGroupTask { get; set; }
        [AddDatabase]
        public string CreatedByEmail { get; set; }
        [AddDatabase]
        public DateTime? CreatedTime { get; set; }

        public ClassModel.User.User CreatedBy { get; set; }
        public List<Process> ListProcess { get; set; } = new List<Process>();

    }
}
