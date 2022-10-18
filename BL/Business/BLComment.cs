using BL.Interface;
using ClassModel.TaskRelate;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace BL.Business
{
    public class BLComment : BLBase, IBLComment
    {
        private IDLComment _iDLComment;
        private ContextRequest _contextRequest;
        public BLComment(IDLComment iDLComment, ContextRequest contextRequest) : base(iDLComment, contextRequest)
        {
            _iDLComment = iDLComment;
            _contextRequest = contextRequest;
        }

        public Comment InsertCustom(Guid taskId, Comment comment)
        {
            comment.CreatedTime = DateTime.Now;
            comment.AttachmentId = taskId;
            comment.CreatedByEmail = _contextRequest.GetEmailCurrentUser();

            _iDLComment.Insert(comment);

            return comment;
        }
    }


}
