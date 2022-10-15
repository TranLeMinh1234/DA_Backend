using ClassModel.Query.SQLBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL
{
    public interface IDLBase
    {
        public Dictionary<string, object> GetPaging(ConditionSqlBuilder conditionSqlBuilder);

        public Guid? Insert<T>(T newRecord);

        public Guid? Update<T>(T record);

        public int Delete<T>(Guid idRecord);

        public T GetById<T>(Guid recordId);

        public List<T> GetAll<T>(string email);

    }
}
