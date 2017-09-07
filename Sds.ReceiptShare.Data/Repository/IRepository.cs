using Sds.ReceiptShare.Domain.Entities;
using System;
using System.Collections.Generic;
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
        IQueryable<T> ReadActive<T>(params string[] linkedObjects) where T : DeletableEntity;

        T Insert<T>(T entity) where T : Entity;
        T InsertManyToMany<T>(T entity) where T : JoiningEntity;

        T Update<T>(T entityToUpdate) where T : Entity;
        void Delete<T>(T entityToUpdate) where T : DeletableEntity;

        IEnumerable<GroupMember> GetGroups(string id);
        void Save();
    }
}
