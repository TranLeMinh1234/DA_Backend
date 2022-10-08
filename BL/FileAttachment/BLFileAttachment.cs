using BL.Interface;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.FileAttachment
{
    public class BLFileAttachment : BLBase,IBLFileAttachment
    {
        public BLFileAttachment(IDLFileAttachment iDLFileAttachment) : base(iDLFileAttachment)
        { 
            
        }
    }
}
