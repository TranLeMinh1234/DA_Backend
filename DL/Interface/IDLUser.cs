﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLUser : IDLBase
    {
        public ClassModel.User.User GetUserInfo(string email);

        public Dictionary<string, object> GetPagingCustom(int from, int take, string searchValue);
    }
}
