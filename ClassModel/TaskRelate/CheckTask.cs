using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.TaskRelate
{
    public class CheckTask
    {
        [PrimaryKey]
        public Guid? CheckTaskId{ get; set; }
        [AddDatabase]
        public string Content{ get; set; }
        [AddDatabase]
        public Boolean Status { get; set; }
        [AddDatabase]
        public string CreatedByEmail { get; set; }
        [AddDatabase]
        public DateTime? CreatedTime { get; set; }
        [AddDatabase]
        public Guid? TaskId { get; set; }
    }
}
