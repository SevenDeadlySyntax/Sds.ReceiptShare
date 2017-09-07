using Sds.ReceiptShare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Data.Repository;
using System.Linq.Expressions;
using System.Linq;

namespace Sds.ReceiptShare.Logic.Managers
{
    /// <summary>
    /// Used to managwe lookup types. Lookup types are used for populating lists. Linked entities will not be returned, as they are not expected to be included in lookup types.
    /// </summary>
    public class LookupManager : Manager, ILookupManager
    {
        public LookupManager(IRepository repository) : base(repository)
        {
        }

        public T Add<T>(T item) where T : LookupEntity
        {
            _repository.Insert<T>(item);
            _repository.Save();
            return item;
        }

        public List<T> GetAll<T>() where T : LookupEntity
        {
            return _repository.Read<T>().ToList();
        }

        public T Get<T>(int id) where T: LookupEntity
        {
            return _repository.Read<T>(id);
        }
    }
}
