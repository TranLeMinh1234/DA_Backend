using ClassModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLLogin : IDLBase
    {
        public bool checkEmailRegisterExists(string Email);
        public User GetUserByEmail(string email);

    }
}
