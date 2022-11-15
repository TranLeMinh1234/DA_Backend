using BL.Interface;
using ClassModel.Notification;
using DL.Business;
using DL.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Notification> GetPagingCustom(string email, int startIndexTake, int numberOfRecordTake) {
            var result = _iDLNotification.GetPagingCustom(email, startIndexTake, numberOfRecordTake);
            if (result?.Count > 0)
            {
                string notificationIds = String.Empty;
                foreach (var notification in result)
                {
                    if (!notification.ReadStatus)
                        notificationIds += $"{notification.NotificationId},";
                }


                if (notificationIds.Length > 1)
                {
                    notificationIds = notificationIds.Remove(notificationIds.Length - 1);
                    _iDLNotification.TickReadNotification(notificationIds);
                }
            }

            return result;
        }

        public int GetNumberOfNewNotification()
        {
            var result = _iDLNotification.GetNumberOfNewNotification(_contextRequest.GetEmailCurrentUser());
            return result;
        }
    }
}
