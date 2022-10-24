using ClassModel.Email;
using ClassModel.TaskRelate;
using ClassModel.User;
using Dapper;
using Org.BouncyCastle.Asn1.BC;
using Quartz;
using Quartz.Impl;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassModel.Enumeration;
using static Service.RemindTaskService;

namespace Service
{
    public class RemindTaskService : IRemindTaskService
    {
        private NameValueCollection configuration = new NameValueCollection() {
            { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
            { "quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz" },
            { "quartz.jobStore.tablePrefix", "QRTZ_" },
            { "quartz.jobStore.dataSource", "tlminh" },
            { "quartz.dataSource.tlminh.connectionString", "Server=DESKTOP-F5IMB2S;Database=tlminh;User=sa;Password=123456;Trusted_Connection=False;TrustServerCertificate=true" },
            { "quartz.dataSource.tlminh.provider", "SqlServer" },
            { "quartz.serializer.type", "json" },
        };

        private StdSchedulerFactory schedulerFactory;

        private IScheduler scheduler;

        protected readonly IMailService _mailService;

        public RemindTaskService()
        {
            /*schedulerFactory = new StdSchedulerFactory(configuration);
            scheduler = (IScheduler)schedulerFactory.GetScheduler();
            scheduler.Start();*/
        }

        public async System.Threading.Tasks.Task InitAsync()
        {
            schedulerFactory = new StdSchedulerFactory(configuration);
            scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();
        }

        public async System.Threading.Tasks.Task AddRemindTaskJob(Guid remindDataStoreId, DateTime timeDeadline)
        {
            try
            {
                if (!scheduler.IsStarted)
                {
                    await scheduler.Start();
                }

                string taskIdentity = $"task_{remindDataStoreId}";
                string triggerIdentity = $"triggerIdentity_{remindDataStoreId}";
                DateTime timeClearTrigger = timeDeadline.AddMinutes(5);

                IJobDetail jobDetail = JobBuilder.Create<RemindTaskJob>()
                    .WithIdentity(taskIdentity, "groupJob")
                    .UsingJobData("JOB_DATA", remindDataStoreId)
                    .Build();

                ITrigger triggerJob = TriggerBuilder.Create()
                    .WithIdentity(triggerIdentity, "groupJob")
                    .StartAt(DateBuilder.DateOf(timeDeadline.Hour, timeDeadline.Minute, timeDeadline.Second, timeDeadline.Day, timeDeadline.Month, timeDeadline.Year))
                    .WithSimpleSchedule(x => x
                        .WithInterval(TimeSpan.FromSeconds(1))
                        .WithRepeatCount(1)
                        .WithMisfireHandlingInstructionNextWithExistingCount())
                    .EndAt(DateBuilder.DateOf(timeClearTrigger.Hour, timeClearTrigger.Minute, timeClearTrigger.Second, timeClearTrigger.Day, timeClearTrigger.Month, timeClearTrigger.Year))
                    .Build();


                await scheduler.ScheduleJob(jobDetail, triggerJob);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteAsync(ex.ToString());
            }
        }

        public class DLService {
            private string connectionString = "" +
               "Host = localhost;" +
               "Port= 3306;" +
               "Database = tlminhdb;" +
               "User Id = root;" +
               "Password = minhbeo2468";
            protected IDbConnection _dbConnection;

            public DLService ()
            {
                _dbConnection = new MySqlConnector.MySqlConnection(connectionString);
            }

            public RemindDataStore GetRemindDataStore(Guid remindDataStoreId)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("RemindDataStoreId", remindDataStoreId);
                var result = _dbConnection.Query<RemindDataStore, ClassModel.TaskRelate.Task, User, RemindDataStore>("Proc_GetRemindDataStore",
                    (remindDataStore, task, user) => {
                        remindDataStore.Task = task;
                        remindDataStore.RemindedUser = user;
                        return remindDataStore;
                    },
                    param,
                    splitOn: "TaskId,UserId", commandType: CommandType.StoredProcedure).FirstOrDefault();
                
                return result;
            }

            ~DLService()
            {
                _dbConnection.Close();
            }
        }

    }
}
