using ClassModel.Email;
using ClassModel.TaskRelate;
using Quartz;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static ClassModel.Enumeration;
using static Service.RemindTaskService;

namespace Service
{
    public class RemindTaskJob : IRemindTaskJob
    {
        public RemindTaskJob()
        {
        }

        public async System.Threading.Tasks.Task Execute(IJobExecutionContext context)
        {
            try
            {
                JobDataMap jobDataMap = context.JobDetail.JobDataMap;

                Guid remindDataStoreId = jobDataMap.GetGuidValue("JOB_DATA");

                DLService dLService = new DLService();

                RemindDataStore remindDataStore = dLService.GetRemindDataStore(remindDataStoreId);

                if (remindDataStore != null && remindDataStore.Task != null)
                {
                    StringBuilder bodyEmail = new StringBuilder();
                    DateTime timeDeadLine = new DateTime();

                    if (remindDataStore.TypeRemind == (int)EnumTypeRemind.EndTime)
                    {
                        timeDeadLine = (DateTime)remindDataStore.Task.EndTime;
                        bodyEmail.Append($"Công việc {remindDataStore.Task.TaskName} đến hạn hoàn thành vào lúc {timeDeadLine.ToString("hh:mm")} ngày {timeDeadLine.ToString("dd/MM/yyyy")}");
                    }
                    else
                    {
                        timeDeadLine = (DateTime)remindDataStore.Task.StartTime;
                        bodyEmail.Append($"Công việc {remindDataStore.Task.TaskName} sẽ bắt đầu vào lúc {timeDeadLine.ToString("hh:mm")} ngày {timeDeadLine.ToString("dd/MM/yyyy")}");
                    }

                    MailRequest mailRequest = new MailRequest()
                    {
                        ToEmail = remindDataStore.EmailRemindedUser,
                        Attachments = null,
                        Body = bodyEmail.ToString(),
                        Subject = "Nhắc nhở công việc",
                    };
                    MailServiceNotInject mailServiceNotInject = new MailServiceNotInject();
                    await mailServiceNotInject.SendEmailAsync(mailRequest);
                    await Console.Out.WriteLineAsync("asdasdasdasdasdasdasdasdasd");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteAsync(ex.ToString());
            }
        }
    }
}
