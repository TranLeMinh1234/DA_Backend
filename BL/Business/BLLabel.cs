using BL.Interface;
using ClassModel.TaskRelate;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Business
{
    public class BLLabel : BLBase, IBLLabel
    {
        private IDLLabel _iDLLabel;
        private ContextRequest _contextRequest;
        public BLLabel(IDLLabel iDLLabel, ContextRequest contextRequest) : base(iDLLabel, contextRequest)
        {
            _iDLLabel = iDLLabel;
            _contextRequest = contextRequest;
        }

        public Label InsertLabelCustom(Label newLabel)
        {
            newLabel.CreatedByEmail = _contextRequest.GetEmailCurrentUser();
            newLabel.CreatedTime = DateTime.Now;

            var newId = _iDLLabel.Insert(newLabel);
            newLabel.LabelId = newId;

            return newLabel;
        }

        public Label UpdateLabelCustom(Label newLabel)
        {
            newLabel.EditByEmail = _contextRequest.GetEmailCurrentUser();
            newLabel.EditTime = DateTime.Now;

            _iDLLabel.Update(newLabel);

            return newLabel;
        }
    }
}
