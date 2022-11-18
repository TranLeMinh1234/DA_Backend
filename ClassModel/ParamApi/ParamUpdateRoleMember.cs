using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.ParamApi
{
    public class ParamUpdateRoleMember
    {
        public string Email { get; set; }
        public Guid GroupTaskId { get; set; }
        public Guid RoleId { get; set; }
        public string NameGroupTask { get; set; }
    }
}
