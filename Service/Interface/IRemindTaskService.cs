using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IRemindTaskService
    {
        public System.Threading.Tasks.Task AddRemindTaskJob(Guid remindDataStoreId, DateTime timeDeadline);
        public System.Threading.Tasks.Task InitAsync(IServiceProvider serviceProvider);
    }
}
