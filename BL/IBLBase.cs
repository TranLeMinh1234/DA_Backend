using ClassModel.Query.SQLBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public interface IBLBase
    {
        public Dictionary<string, object> GetPaging();

        public Guid Insert<T>(T newRecord);

        public Guid Update<T>(T record);

        public int Delete<T>(Guid idRecord);

        public T GetById<T>(Guid recordId);
    }
}
