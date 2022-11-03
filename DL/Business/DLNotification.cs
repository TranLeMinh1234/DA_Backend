using ClassModel.Notification;
using ClassModel.TaskRelate;
using ClassModel.User;
using Dapper;
using DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL.Business
{
    public class DLNotification : DLBase, IDLNotification
    {
        public int InsertBatch(List<Notification> listNotification)
        {
            StringBuilder sqlValuesInsert = new StringBuilder();
            Dictionary<string, object> param = new Dictionary<string, object>();
            for (int i = 1; i <= listNotification.Count; i++)
            {
                sqlValuesInsert.Append($"(@NotificationId{i}, @TypeNoti{i}, @CreatedByEmail{i}, @NotifyForEmail{i}, @TaskRelateId{i}, @GroupTaskRelateId{i}, @CreatedTime{i}),");
                param.Add($"NotificationId{i}", Guid.NewGuid());
                param.Add($"TypeNoti{i}", listNotification.ElementAt(i - 1).TypeNoti);
                param.Add($"CreatedByEmail{i}", listNotification.ElementAt(i - 1).CreatedByEmail);
                param.Add($"NotifyForEmail{i}", listNotification.ElementAt(i - 1).NotifyForEmail);
                param.Add($"TaskRelateId{i}", listNotification.ElementAt(i - 1).TaskRelateId);
                param.Add($"GroupTaskRelateId{i}", listNotification.ElementAt(i - 1).GroupTaskRelateId);
                param.Add($"CreatedTime{i}", listNotification.ElementAt(i - 1).CreatedTime);
            }

            sqlValuesInsert = sqlValuesInsert.Remove(sqlValuesInsert.Length - 1, 1);

            string sql = $"INSERT INTO Notification (NotificationId,TypeNoti,CreatedByEmail,NotifyForEmail,TaskRelateId,GroupTaskRelateId,CreatedTime) VALUES {sqlValuesInsert.ToString()};";

            var result = _dbConnection.Execute(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public List<Notification> GetPagingCustom(string email, int startIndexTake, int numberOfRecordTake) {
            
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("EmailQuery", email);
            param.Add("StartIndexTake", startIndexTake);
            param.Add("NumberOfRecordTake", numberOfRecordTake);

            var result = _dbConnection.Query<Notification, User, Task, GroupTask, Notification>("Proc_GetPagingNotification",
                (notification, user, task, groupTask) =>
                {
                    notification.CreatedBy = user;
                    notification.Task = task;
                    notification.GroupTask = groupTask;

                    return notification;
                }
                , param, splitOn: "Email,TaskId,GroupTaskId", commandType: System.Data.CommandType.StoredProcedure).AsList();
            
            return result;
        }
    }
}
