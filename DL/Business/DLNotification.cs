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
                sqlValuesInsert.Append($"(@NotificationId{i}, @TypeNoti{i}, @CreatedByEmail{i}, @NotifyForEmail{i}, @TaskRelateId{i}, @GroupTaskRelateId{i}, @CreatedTime{i},@RoleRelateId{i}, @ReadStatus{i}, @NameGroupTask{i}),");
                param.Add($"NotificationId{i}", Guid.NewGuid());
                param.Add($"TypeNoti{i}", listNotification.ElementAt(i - 1).TypeNoti);
                param.Add($"CreatedByEmail{i}", listNotification.ElementAt(i - 1).CreatedByEmail);
                param.Add($"NotifyForEmail{i}", listNotification.ElementAt(i - 1).NotifyForEmail);
                param.Add($"TaskRelateId{i}", listNotification.ElementAt(i - 1).TaskRelateId);
                param.Add($"GroupTaskRelateId{i}", listNotification.ElementAt(i - 1).GroupTaskRelateId);
                param.Add($"CreatedTime{i}", listNotification.ElementAt(i - 1).CreatedTime);
                param.Add($"RoleRelateId{i}", listNotification.ElementAt(i - 1).RoleRelateId);
                param.Add($"ReadStatus{i}", listNotification.ElementAt(i - 1).ReadStatus);
                param.Add($"NameGroupTask{i}", listNotification.ElementAt(i - 1).NameGroupTask);
            }

            sqlValuesInsert = sqlValuesInsert.Remove(sqlValuesInsert.Length - 1, 1);

            string sql = $"INSERT INTO Notification (NotificationId,TypeNoti,CreatedByEmail,NotifyForEmail,TaskRelateId,GroupTaskRelateId,CreatedTime,RoleRelateId,ReadStatus,NameGroupTask) VALUES {sqlValuesInsert.ToString()};";

            var result = _dbConnection.Execute(sql, param, commandType: System.Data.CommandType.Text);
            return result;
        }

        public List<Notification> GetPagingCustom(string email, int startIndexTake, int numberOfRecordTake) {
            
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("EmailQuery", email);
            param.Add("StartIndexTake", startIndexTake);
            param.Add("NumberOfRecordTake", numberOfRecordTake);

            var result = _dbConnection.Query<Notification, User, Task, GroupTask,Role, Notification>("Proc_GetPagingNotification",
                (notification, user, task, groupTask, role) =>
                {
                    notification.CreatedBy = user;
                    notification.Task = task;
                    notification.GroupTask = groupTask;
                    notification.Role = role;

                    return notification;
                }
                , param, splitOn: "Email,TaskId,GroupTaskId,RoleId", commandType: System.Data.CommandType.StoredProcedure).AsList();
            
            return result;
        }

        public int TickReadNotification(string notificationIds) {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("NotificationIds", notificationIds);
            var result = _dbConnection.Execute("Proc_TickReadNotification", param, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public int GetNumberOfNewNotification(string email)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Email", email);
            string sql = "SELECT COUNT(*) FROM Notification WHERE NotifyForEmail = @Email AND ReadStatus = false";
            var result = _dbConnection.Query<int>(sql, param, commandType: System.Data.CommandType.Text).FirstOrDefault();
            return result;
        }
    }
}
