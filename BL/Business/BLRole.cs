using BL.Interface;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Business
{
    public class BLRole : BLBase, IBLRole
    {
        private IDLRole _iDLRole;
        private ContextRequest _contextRequest;

        public BLRole(IDLRole iDLRole, ContextRequest contextRequest) : base(iDLRole, contextRequest)
        {
            _iDLRole = iDLRole;
            _contextRequest = contextRequest;
        }
    }
}
