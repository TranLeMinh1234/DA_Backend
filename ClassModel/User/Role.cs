using Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.User
{
    public class Role
    {
        [PrimaryKey]
        public Guid? RoleId { get; set; }
        [AddDatabase]
        public string NameRole { get; set; }
        [AddDatabase]
        public Guid? RoleGroupTaskReferenceId { get; set; }
        [AddDatabase]
        public string ListPermissionCode { get; set; }
        [AddDatabase]
        public DateTime? CreatedTime { get; set; }
        [AddDatabase]
        public string CreatedByEmail { get; set; }
        [AddDatabase]
        public string Description { get; set; }

        public List<Permission> ListPermission { get; set; } = new List<Permission>();

    }
}
