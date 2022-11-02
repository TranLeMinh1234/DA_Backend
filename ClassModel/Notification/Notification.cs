using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.Notification
{
    public class Notification
    {
        [PrimaryKey]
        public Guid? NotificationId { get; set; }
        [AddDatabase]
        public int TypeNoti { get; set; }
        [AddDatabase]
        public DateTime? CreatedTime { get; set; }
        [AddDatabase]
        public string CreatedByEmail { get; set; }
        [AddDatabase]
        public string NotifyForEmail { get; set; }
        [AddDatabase]
        public Guid? TaskRelateId { get; set; }
        [AddDatabase]
        public Guid? GroupTaskRelateId { get; set; }

        public ClassModel.TaskRelate.GroupTask GroupTask { get; set; }
        public ClassModel.TaskRelate.Task Task { get; set; }
        public ClassModel.User.User NotifyForUser { get; set; }
        public ClassModel.User.User CreatedBy { get; set; }

    }
}
