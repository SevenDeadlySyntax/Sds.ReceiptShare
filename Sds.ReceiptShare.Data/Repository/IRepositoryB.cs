using Sds.ReceiptShare.Domain.Entities;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Data.Repository
{
    public interface IRepositoryB<T> where T : Entity
    {
        T Get(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}