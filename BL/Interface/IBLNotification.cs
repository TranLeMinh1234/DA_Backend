using ClassModel.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interface
{
    public interface IBLNotification: IBLBase
    {
        public int InsertBatch(List<Notification> listNotification);
        public List<Notification> GetPagingCustom(string email, int startIndexTake, int numberOfRecordTake);
        public int GetNumberOfNewNotification();
    }
}
