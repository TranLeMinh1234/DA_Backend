using Attribute;
using ClassModel.File;
using System;
using System.Collections.Generic;
using System.Text;
using static ClassModel.Enumeration;

namespace ClassModel.TaskRelate
{
    public class Task
    {
        [PrimaryKey]
        public Guid? TaskId { get; set; }
        [AddDatabase]
        public string TaskName { get; set; }
        [AddDatabase]
        public EnumTypeTask TypeTask { get; set; }
        [AddDatabase]
        public string Description { get; set; }
        [AddDatabase]
        public string AssignedByEmail { get; set; }
        [AddDatabase]
        public string CreatedByEmail { get; set; }
        [AddDatabase]
        public DateTime? CreatedTime { get; set; }
        [AddDatabase]
        public string PathTreeTask { get; set; }
        [AddDatabase]
        public DateTime? StartTime { get; set; }
        [AddDatabase]
        public DateTime? EndTime { get; set; }
        [AddDatabase]
        public int? SortOrder { get; set; }
        [AddDatabase]
        public Guid? ProcessId { get; set; }
        [AddDatabase]
        public string AssignForEmail { get; set; }
        [AddDatabase]
        public Guid? GroupTaskId { get; set; }

        public List<FileAttachment> ListAttachment { get; set; }
        public List<string> ListCommment { get; set; }
        public List<CheckTask> ListCheckTask { get; set; }
        public List<Label> ListLabel { get; set; }
        public ClassModel.User.User CreatedBy { get; set; }
        public ClassModel.User.User AssignedBy { get; set; }
        public ClassModel.User.User AssignedFor { get; set; }
    }
}
