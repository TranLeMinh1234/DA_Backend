using DL;
using System;
using System.Collections.Generic;

namespace BL
{
    public class BLBase : IBLBase
    {
        protected IDLBase _dlBase;
        public BLBase(IDLBase dLBase)
        {
            _dlBase = dLBase;
        }

        public int Delete<T>(Guid idRecord)
        {
            return _dlBase.Delete<T>(idRecord);
        }

        public T GetById<T>(Guid recordId)
        {
            T record = _dlBase.GetById<T>(recordId);
            return record;
        }

        public Dictionary<string, object> GetPaging()
        {
            throw new NotImplementedException();
        }

        public Guid? Insert<T>(T newRecord)
        {
            Guid? newId = _dlBase.Insert(newRecord);
            return newId;
        }

        public Guid? Update<T>(T record)
        {
            Guid? newId = _dlBase.Update(record);
            return newId;
        }
    }
}
