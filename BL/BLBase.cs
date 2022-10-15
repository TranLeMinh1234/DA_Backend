using DL;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace BL
{
    public class BLBase : IBLBase
    {
        protected IDLBase _dlBase;
        protected ContextRequest _contextRequest;
        public BLBase(IDLBase dLBase, ContextRequest contextRequest)
        {
            _dlBase = dLBase;
            _contextRequest = contextRequest;
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

        public List<T> GetAll<T>()
        {
            var emailUser = _contextRequest.GetEmailCurrentUser();
            var result = _dlBase.GetAll<T>(emailUser);
            return result;
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
