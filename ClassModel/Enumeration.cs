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
    }
}
