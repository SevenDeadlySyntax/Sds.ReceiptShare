using System.Collections.Generic;
using Sds.ReceiptShare.Domain.Entities;

namespace Sds.ReceiptShare.Logic.Interfaces
{
    public interface ILookupManager
    {
        T Add<T>(T item) where T : LookupEntity;
        T Get<T>(int id) where T : LookupEntity;
        List<T> GetAll<T>() where T : LookupEntity;
    }
}