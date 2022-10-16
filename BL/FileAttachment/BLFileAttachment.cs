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
        IDLFileAttachment _iDLFileAttachment;
        public BLFileAttachment(IDLFileAttachment iDLFileAttachment, ContextRequest contextRequest) : base(iDLFileAttachment, contextRequest)
        { 
            _iDLFileAttachment = iDLFileAttachment;
            _contextRequest = contextRequest;
        }

        public List<ClassModel.File.FileAttachment> GetAttachFile(Guid taskId)
        {
            var result = _iDLFileAttachment.GetAttachFile(taskId);
            return result;
        }
    }
}
