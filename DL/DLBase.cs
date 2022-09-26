using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using Utilities;

namespace DL
{
    public class DLBase : IDLBase
    {
        public int Delete<T>(Guid idRecord, T instanceDelete)
        {
            Type type = instanceDelete.GetType();
            string tableName = type.Name;
            PropertyInfo primaryKey = ClassUtility.GetPrimaryKeyPropertyOfClass(type);

            Dictionary<string, object> paramDelete = new Dictionary<string, object>();
            paramDelete.Add(primaryKey.Name, idRecord);
            string sqlDelete = $"DELETE FROM {tableName} WHERE {primaryKey.Name} = @{primaryKey.Name}";

            return 0;
        }

        public List<object> GetPaging(int start, int take)
        {
            throw new NotImplementedException();
        }

        public Guid Insert<T>(T newRecord)
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

            string sql = $"INSERT INTO {tableName} ({stringColumnInsert}) VALUES {stringParamInsert};";


            return (Guid)newID;
        }

        public Guid Update<T>(T record)
        {
            throw new NotImplementedException();
        }
    }
}
