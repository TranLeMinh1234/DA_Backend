using System;
using System.Collections.Generic;
using System.Text;

namespace DL
{
    public interface IDLBase
    {
        public List<object> GetPaging(int start, int take);

        public Guid Insert<T>(T newRecord);

        public Guid Update<T>(T record);

        public int Delete<T>(Guid idRecord, T instanceDelete);
        
    }
}
