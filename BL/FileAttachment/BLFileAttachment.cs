using BL.Interface;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.FileAttachment
{
    public class BLFileAttachment : BLBase,IBLFileAttachment
    {
        private ContextRequest _contextRequest;
        public BLFileAttachment(IDLFileAttachment iDLFileAttachment, ContextRequest contextRequest) : base(iDLFileAttachment, contextRequest)
        { 
            
        }
    }
}
