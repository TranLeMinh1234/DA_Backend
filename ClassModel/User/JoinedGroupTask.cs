using Attribute;
using ClassModel.TaskRelate;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.User
{
    public class JoinedGroupTask
    {
        [PrimaryKey]
        public Guid? JoinId { get; set; }
        [AddDatabase]
        public string UserJoinedEmail { get; set; }
        [AddDatabase]
        public string InvitedByEmail { get; set; }
        [AddDatabase]
        public Guid? RoleReferenceId { get; set; }
        [AddDatabase]
        public Guid? GroupTaskReferenceId { get; set; }
        [AddDatabase]
        public DateTime? JoinedTime { get; set; }
        public User InvitedBy { get; set; }
        public User UserJoined { get; set; }
        public GroupTask GroupTask { get; set; }
        public Role Role { get; set; }
    }
}
