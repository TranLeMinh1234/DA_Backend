using ClassModel.Email;
using ClassModel.TaskRelate;
using ClassModel.User;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.BC;
using Quartz;
using Quartz.Impl;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities;
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

        public async System.Threading.Tasks.Task InitAsync(IServiceProvider serviceProvider)
        {
            schedulerFactory = new StdSchedulerFactory(configuration);
            scheduler = await schedulerFactory.GetScheduler();
            scheduler.JobFactory = new InjectJobFactory(serviceProvider);
            
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

            public ClassModel.TaskRelate.Task GetFullInfo(Guid taskId)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TaskIdParam", taskId);
                var result = _dbConnection.Query<ClassModel.TaskRelate.Task, User, User, User, ClassModel.TaskRelate.Task>("Proc_GetFullInfoTask",
                    map: (task, userAssign, userDoTask, userCreate) =>
                    {
                        task.AssignedBy = userAssign;
                        task.AssignedFor = userDoTask;
                        task.CreatedBy = userCreate;

                        return task;
                    },
                    param,
                    splitOn: "Email,Email,Email",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                return result;
            }

            public Guid? Insert<T>(T newRecord)
            {
                Type typeRecord = newRecord.GetType();
                string tableName = newRecord.GetType().Name;
                PropertyInfo primaryKeyProperty = ClassUtility.GetPrimaryKeyPropertyOfClass(typeRecord);
                List<string> namePropertiesInsertDb = ClassUtility.GetNamePropertiesInsertDb(typeRecord);
                Dictionary<string, object> paramInsert = ClassUtility.GetPropertiesInsertDb(typeRecord, newRecord);
                object newID = null;
                paramInsert.TryGetValue(primaryKeyProperty.Name, out newID);

                string stringColumnInsert = "";
                string stringParamInsert = "";
                foreach (string nameColumn in namePropertiesInsertDb)
                {
                    stringColumnInsert += nameColumn + ", ";
                    stringParamInsert += $"@{nameColumn}, ";
                }

                stringColumnInsert = stringColumnInsert.Remove(stringColumnInsert.Length - 2);
                stringParamInsert = stringParamInsert.Remove(stringParamInsert.Length - 2);

                string sql = $"INSERT INTO {tableName} ({stringColumnInsert}) VALUES ({stringParamInsert});";

                int result = _dbConnection.Execute(sql, paramInsert, commandType: CommandType.Text);
                if (result != 0)
                {
                    return (Guid)newID;
                }
                else
                    return null;
            }

            public T GetById<T>(Guid recordId)
            {
                Type type = typeof(T);
                string tableName = type.Name;
                PropertyInfo propertyPrimaryKey = ClassUtility.GetPrimaryKeyPropertyOfClass(type);
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add(propertyPrimaryKey.Name, recordId);
                string sqlQuery = $"SELECT * FROM {tableName} WHERE {propertyPrimaryKey.Name} = @{propertyPrimaryKey.Name}";

                return _dbConnection.Query<T>(sqlQuery, param, commandType: CommandType.Text).FirstOrDefault();
            }


            ~DLService()
            {
                _dbConnection.Close();
            }
        }

    }
}
