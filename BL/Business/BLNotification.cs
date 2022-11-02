using BL.Interface;
using ClassModel.Notification;
using DL.Business;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Business
{
    public class BLNotification : BLBase, IBLNotification
    {
        private IDLNotification _iDLNotification;
        private ContextRequest _contextRequest;

        public BLNotification(IDLNotification iDLNotification, ContextRequest contextRequest) : base(iDLNotification, contextRequest)
        {
            _iDLNotification = iDLNotification;
            _contextRequest = contextRequest;
        }

        public int InsertBatch(List<Notification> listNotification)
        {
            var result = _iDLNotification.InsertBatch(listNotification);
            return result;
        }
    }
}
