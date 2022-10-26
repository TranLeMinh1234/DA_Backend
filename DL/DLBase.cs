using Attribute;
using ClassModel.Query.SQLBuilder;
using ClassModel.User;
using Dapper;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using Utilities;

namespace DL
{
    public class DLBase : DBContext, IDLBase
    {
        public DLBase() : base()
        { 
            
        }
        public int Delete<T>(Guid idRecord)
        {
            Type type = typeof(T);
            string tableName = type.Name;
            PropertyInfo primaryKey = ClassUtility.GetPrimaryKeyPropertyOfClass(type);

            Dictionary<string, object> paramDelete = new Dictionary<string, object>();
            paramDelete.Add(primaryKey.Name, idRecord);
            string sqlDelete = $"DELETE FROM {tableName} WHERE {primaryKey.Name} = @{primaryKey.Name};";

            return _dbConnection.Execute(sqlDelete, paramDelete, commandType: CommandType.Text);
        }

        public Dictionary<string,object> GetPaging(ConditionSqlBuilder conditionSqlBuilder)
        {
            Dictionary<string, object> resultPaging = new Dictionary<string, object>();
            string sql = conditionSqlBuilder.BuildPagingSql();
            Dictionary<string, object> param = conditionSqlBuilder.BuildParamCondition();

            return null;
        }

        public Guid? Insert<T>(T newRecord)
        {
            Type typeRecord = newRecord.GetType();
            string tableName = newRecord.GetType().Name;
            PropertyInfo primaryKeyProperty = ClassUtility.GetPrimaryKeyPropertyOfClass(typeRecord);
            List<string> namePropertiesInsertDb = ClassUtility.GetNamePropertiesInsertDb(typeRecord);
            Dictionary<string, object> paramInsert = ClassUtility.GetPropertiesInsertDb(typeRecord,newRecord);
            object newID = null;
            paramInsert.TryGetValue(primaryKeyProperty.Name,out newID);

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

            int result = _dbConnection.Execute(sql,paramInsert,commandType: CommandType.Text);
            if (result != 0)
            { 
                return (Guid)newID;
            }
            else
                return null;
        }

        public Guid? Update<T>(T record)
        {
            Type typeRecord = record.GetType();
            string tableName = record.GetType().Name;
            PropertyInfo primaryKeyProperty = ClassUtility.GetPrimaryKeyPropertyOfClass(typeRecord);
            List<string> namePropertiesInsertDb = ClassUtility.GetNamePropertiesInsertDb(typeRecord);
            Dictionary<string, object> paramInsert = ClassUtility.GetPropertiesInsertDb(typeRecord, record);
            object idRecord = null;
            paramInsert.TryGetValue(primaryKeyProperty.Name, out idRecord);

            string stringPairColumnUpdate = "";

            foreach(string nameColumnUpdate in namePropertiesInsertDb)
            {
                stringPairColumnUpdate += $" {nameColumnUpdate} = @{nameColumnUpdate},";
            }

            stringPairColumnUpdate = stringPairColumnUpdate.Remove(stringPairColumnUpdate.Length - 1);

            string sql = $"UPDATE {tableName} SET {stringPairColumnUpdate} WHERE {primaryKeyProperty.Name} = @{primaryKeyProperty.Name}";

            int result = _dbConnection.Execute(sql, paramInsert, commandType: CommandType.Text);

            if (result != 0)
            {
                return (Guid)idRecord;
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

            return _dbConnection.Query<T>(sqlQuery,param, commandType: CommandType.Text).FirstOrDefault();
        }

        public List<T> GetAll<T>(string email)
        {
            Type type = typeof(T);
            string tableName = type.Name;
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("CreatedByEmail", email);
            string sqlQuery = $"SELECT * FROM {tableName} WHERE CreatedByEmail = @CreatedByEmail";

            return (List<T>)_dbConnection.Query<T>(sqlQuery, param, commandType: CommandType.Text);
        }

        public User GetUserInfo(string emailQuery)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("EmailQuery", emailQuery);
            var result = _dbConnection.Query<User>("Proc_GetUserInfo", param, commandType: CommandType.StoredProcedure).FirstOrDefault(); ;
            return result;
        }

        ~DLBase()
        {
            _dbConnection.Close();
        }
    }
}
