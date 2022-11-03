using ClassModel.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Interface
{
    public interface IDLNotification : IDLBase
    {
        public int InsertBatch(List<Notification> listNotification);
        public List<Notification> GetPagingCustom(string email, int startIndexTake, int numberOfRecordTake);
    }
}
