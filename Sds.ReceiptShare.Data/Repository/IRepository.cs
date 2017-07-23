using Sds.ReceiptShare.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sds.ReceiptShare.Data.Repository
{
    public interface IRepository
    {
        IQueryable<T> Read<T>() where T : Entity;
        T Read<T>(int id, params string[] linkedObjects) where T : Entity;
        IQueryable<T> Read<T>(params string[] linkedObjects) where T : Entity;
        IQueryable<T> Read<T>(Expression<Func<T, bool>> query, params string[] linkedObjects) where T : Entity;
        
        T Insert<T>(T entity) where T : Entity;
        T Update<T>(T entityToUpdate) where T : Entity;
        void Delete<T>(T entityToUpdate) where T : DeletableEntity;

        void Save();
    }
}
