using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLUser : IBLBase
    {
        public ClassModel.User.User GetUserInfo();
    }
}
