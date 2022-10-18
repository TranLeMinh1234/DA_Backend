using Attribute;
using ClassModel.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassModel.User
{
    public class User
    {
        [PrimaryKey]
        public Guid? UserId { get; set; }
        [AddDatabase]
        public string LastName { get; set; }
        [AddDatabase]
        public string FirstName { get; set; }
        [AddDatabase]
        public string Email { get; set; }
        [AddDatabase]
        public string PhoneNumber { get; set; }
        [AddDatabase]
        public string PassWord { get; set; }
        [AddDatabase]
        public DateTime? DateOfBirth { get; set; }
        public FileAttachment FileAvatar { get; set; } = null;
        public string FileAvatarName { get; set; }
        public string PassWordRepeated { get; set; }
    }
}
