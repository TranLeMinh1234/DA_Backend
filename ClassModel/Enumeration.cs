using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel
{
    public class Enumeration
    {
        public enum EnumAttachment
        {
            AttachAvatar = 1,
            AttachTask = 2,
            AttachComment = 3,
        }

        public enum EnumTypeTask
        {
            Personal = 1,
            GroupPersonal = 2,
            Group = 3
        }

        public enum EnumTypeDeadline
        {
            Start = 0,
            End = 1
        }

        public enum EnumTypeRemind
        {
            StartTime = 0,
            EndTime = 1
        }

        public enum EnumTypeGroupTask
        {
            Personal = 1,
            Group = 2
        }

        public enum EnumTypeNotification { 
            AddUserGroupTask = 1,
            DeleteUserFromGroupTask = 2,
            AssignedTask = 3,
            DeletedTask = 4,
            CommentedTask = 5,
            RemindTask = 6,
            DeleteGroupTask = 7,
            ReloadGroupTask = 8,
            RemindEndTimeTask = 9,
            RemindStartTimeTask = 10,
            ChangeRoleGroupTask = 11
        }

        public enum EnumStatusTask { 
            NeedExecute = 1,
            CheckFinished = 2,
            ConfirmedFinished = 3
        }
    }
}
