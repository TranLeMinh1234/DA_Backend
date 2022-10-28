using BL.Interface;
using DL.Business;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Business
{
    public class BLUser : BLBase, IBLUser
    {
        private IDLUser _iDLUser;
        private ContextRequest _contextRequest;

        public BLUser(IDLUser iDLUser, ContextRequest contextRequest) : base(iDLUser, contextRequest)
        {
            _iDLUser = iDLUser;
            _contextRequest = contextRequest;
        }

        public ClassModel.User.User GetUserInfo() {
            var result = _iDLUser.GetUserInfo(_contextRequest.GetEmailCurrentUser());
            if (result != null)
            {
                result.UserId = null;
            }
            return result;
        }

        public Dictionary<string, object> GetPagingCustom(int from, int take, string searchValue) {
            
            var result = _iDLUser.GetPagingCustom(from, take, searchValue);
            return result;
        }
    }
}
