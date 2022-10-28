using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLUser : IBLBase
    {
        public ClassModel.User.User GetUserInfo();

        public Dictionary<string, object> GetPagingCustom(int from, int take, string searchValue);
    }
}
