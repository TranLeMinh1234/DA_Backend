using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.TaskRelate
{
    public class RemindDataStore
    {
        [PrimaryKey]
        public Guid? RemindDataId { get; set; }
        [AddDatabase]
        public int TypeRemind { get; set; }
        [AddDatabase]
        public Boolean IsUsed { get; set; }
        [AddDatabase]
        public string EmailRemindedUser { get; set; }
        [AddDatabase]
        public Guid? TaskId { get; set; }


        public ClassModel.User.User RemindedUser { get; set; }
        public Task Task { get; set; }

    }
}
