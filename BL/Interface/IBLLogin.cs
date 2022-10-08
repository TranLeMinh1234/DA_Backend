using ClassModel;
using ClassModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLLogin : IBLBase
    {
        public ServiceResult Register(User newUser);
    }
}
