using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.TaskRelate
{
    public class Process
    {
        [PrimaryKey]
        public Guid? ProcessId { get; set; }
        [AddDatabase]
        public string ProcessName { get; set; }
        [AddDatabase]
        public int? SortOrder { get; set; }
        [AddDatabase]
        public string Description { get; set; }
        [AddDatabase]
        public DateTime? CreatedTime { get; set; }
        [AddDatabase]
        public Guid? TemplateGroupTaskReferenceId { get; set; }
        [AddDatabase]
        public Guid? ColumnSettingReferenceId { get; set; }
        [AddDatabase]
        public string CreatedByEmail { get; set; }

        public ColumnSetting ColumnSetting { get; set; }
        public ClassModel.User.User CreatedBy { get; set; }
    }
}
