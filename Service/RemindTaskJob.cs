using ClassModel.Email;
using ClassModel.Notification;
using ClassModel.TaskRelate;
using Newtonsoft.Json;
using Quartz;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ClassModel.Enumeration;
using static Service.RemindTaskService;

namespace Service
{
    public class RemindTaskJob : IRemindTaskJob
    {
        private WebsocketConnectionManager _websocketConnectionManager;
        public RemindTaskJob(WebsocketConnectionManager websocketConnectionManager)
        {
            _websocketConnectionManager = websocketConnectionManager;   
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
                    GroupTask groupTask = dLService.GetById<GroupTask>((Guid)remindDataStore.Task.GroupTaskId);
                    StringBuilder bodyEmail = new StringBuilder();
                    DateTime timeDeadLine = new DateTime();


                    if (remindDataStore.TypeRemind == (int)EnumTypeRemind.EndTime)
                    {
                        timeDeadLine = (DateTime)remindDataStore.Task.EndTime;
                        bodyEmail.Append($"Công việc <b>{remindDataStore.Task.TaskName}</b> đến hạn hoàn thành vào lúc <b>{timeDeadLine.ToString("hh:mm")}</b> ngày <b>{timeDeadLine.ToString("dd/MM/yyyy")}</b>");
                        
                        bodyEmail.Append("<br>");
                        if (groupTask != null && (remindDataStore.Task.TypeTask == EnumTypeTask.Group || remindDataStore.Task.TypeTask == EnumTypeTask.GroupPersonal))
                            bodyEmail.Append($"http://localhost:8080/DetailGroupTask/{remindDataStore.Task.GroupTaskId}/{groupTask.TemplateReferenceId}/{groupTask.TypeGroupTask}/{remindDataStore.Task.TaskId}");
                        else
                        {
                            bodyEmail.Append($"http://localhost:8080/DailyTask/{remindDataStore.Task.TaskId}");
                        }
                    }
                    else if(remindDataStore.TypeRemind == (int)EnumTypeRemind.StartTime)
                    {
                        timeDeadLine = (DateTime)remindDataStore.Task.StartTime;
                        bodyEmail.Append($"Công việc <b>{remindDataStore.Task.TaskName}</b> sẽ bắt đầu vào lúc <b>{timeDeadLine.ToString("hh:mm")}</b> ngày <b>{timeDeadLine.ToString("dd/MM/yyyy")}</b>");

                        bodyEmail.Append("<br>");
                        if (groupTask != null && (remindDataStore.Task.TypeTask == EnumTypeTask.Group || remindDataStore.Task.TypeTask == EnumTypeTask.GroupPersonal))
                            bodyEmail.Append($"http://localhost:8080/DetailGroupTask/{remindDataStore.Task.GroupTaskId}/{groupTask.TemplateReferenceId}/{groupTask.TypeGroupTask}/{remindDataStore.Task.TaskId}");
                        else
                        {
                            bodyEmail.Append($"http://localhost:8080/DailyTask/{remindDataStore.Task.TaskId}");
                        }
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

                    Notification notification = new Notification()
                    {
                        CreatedByEmail = null,
                        NotificationId = null,
                        GroupTaskRelateId = remindDataStore.Task.GroupTaskId,
                        NotifyForEmail = remindDataStore.EmailRemindedUser,
                        TaskRelateId = remindDataStore.Task.TaskId,
                        Task = remindDataStore.Task,
                        TypeNoti = remindDataStore.TypeRemind == (int)EnumTypeRemind.EndTime? (int)EnumTypeNotification.RemindEndTimeTask : (int)EnumTypeNotification.RemindStartTimeTask,
                        CreatedTime = DateTime.Now,
                        TaskName = remindDataStore.Task.TaskName,
                        ReadStatus = false
                    };

                    dLService.Insert(notification);
                    await System.Threading.Tasks.Task.Run(() => _websocketConnectionManager.SendMessageToUser(remindDataStore.EmailRemindedUser, JsonConvert.SerializeObject(notification)));
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteAsync(ex.ToString());
            }
        }
    }
}
