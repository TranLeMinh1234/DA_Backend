using Attribute;
using ClassModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.TaskRelate
{
    public class GroupTask
    {
        [PrimaryKey]
        public Guid? GroupTaskId { get; set; }
        [AddDatabase]
        public string NameGroupTask { get; set; }
        [AddDatabase]
        public string CreatedByEmail { get; set; }
        [AddDatabase]
        public DateTime? CreatedTime { get; set; }
        [AddDatabase]
        public string Description { get; set; }
        [AddDatabase]
        public int TypeGroupTask { get; set; }
        [AddDatabase]
        public Guid? TemplateReferenceId { get; set; }

        public List<Role> ListRole { get; set; } = new List<Role>();
        public TemplateCustom TemplateCustom { get; set; }
        public ClassModel.User.User CreatedBy { get; set; }
        public List<Task> ListTask { get; set; }
    }
}
