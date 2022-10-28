using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.User
{
    public class Permission
    {
        public Guid? PermissionId { get; set; }
        public string Content { get; set; }
        public string PermissionCode { get; set; }

    }
}
