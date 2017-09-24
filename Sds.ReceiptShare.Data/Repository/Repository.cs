using Microsoft.EntityFrameworkCore;
using Sds.ReceiptShare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Sds.ReceiptShare.Data.Repository
{
    public class Repository : IRepository
    {
        private DataContext _context;

        public Repository(DataContext context)
        {
            this._context = context;
        }

        public virtual IQueryable<T> Read<T>() where T : Entity
        {
            IQueryable<T> query = _context.Set<T>();
            return query;
        }

        public virtual T Read<T>(int id, params string[] linkedObjects) where T : Entity
        {
            return Read<T>(linkedObjects).AsNoTracking().SingleOrDefault(s => s.Id == id);
        }

        public virtual IQueryable<T> ReadActive<T>(params string[] linkedObjects) where T : DeletableEntity
        {
            return this.Read<T>(linkedObjects).Where(s=> !s.IsDeleted);            
        }

        public virtual IQueryable<T> Read<T>(params string[] linkedObjects) where T : Entity
        {
            var query = this.Read<T>();
            return linkedObjects.Where(objectType => objectType != string.Empty).Aggregate(query, (current, objectType) => current.Include(objectType)).AsNoTracking();
        }

        public virtual IQueryable<T> Read<T>(Expression<Func<T, bool>> query, params string[] linkedObjects) where T : Entity
        {
            return this.Read<T>(linkedObjects).Where(query);
        }

        public virtual T Insert<T>(T entity) where T : Entity
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"Entity {entity.GetType()}");
            }

            entity.Created = DateTime.Now;
            var entities = _context.Set<T>();
            var newEntity = entities.Add(entity);
            return newEntity.Entity;
        }

        public virtual T InsertManyToMany<T>(T entity) where T : JoiningEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"Entity {entity.GetType()}");
            }

            var entities = _context.Set<T>();
            var newEntity = entities.Add(entity);
            return newEntity.Entity;
        }

        public virtual T Update<T>(T entityToUpdate) where T : Entity
        {
            entityToUpdate.Updated = DateTime.Now;
            return UpdateAny<T>(entityToUpdate);
        }

        public virtual T UpdateManyToMany<T>(T entityToUpdate) where T : JoiningEntity
        {
            return UpdateAny<T>(entityToUpdate);
        }

        private T UpdateAny<T>(T entityToUpdate) where T : class
        {
            var entities = _context.Set<T>();            
            entities.Update(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return entityToUpdate;
        }

        public void Delete<T>(T entityToDelete) where T : DeletableEntity
        {
            entityToDelete.Deleted = DateTime.Now;
            entityToDelete.IsDeleted = true;
            this.Update<T>(entityToDelete);
        }

        public virtual T DeleteManyToMany<T>(T entity) where T : JoiningEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"Entity {entity.GetType()}");
            }
            
            var entities = _context.Set<T>();
            var newEntity = entities.Remove(entity);
            return newEntity.Entity;
        }

        public void Save()
        {
            _context.SaveChanges();           
            
        }

        public virtual IEnumerable<GroupMember> GetGroups(string id)
        {
            return _context.Set<ApplicationUser>().Include("Groups").Include("Groups.Group").SingleOrDefault(s => s.Id == id)?.Groups;
        }
    }
}
    // Alternative implementation
    //public class RepositoryB<T> : IRepositoryB<T> where T : Entity
    //{
    //    private readonly DataContext context;
    //    string errorMessage = string.Empty;
    //    private DbSet<T> entities;

    //    public RepositoryB(DataContext context)
    //    {
    //        this.context = context;
    //        entities = context.Set<T>();
    //    }

    //    public T Get(int id)
    //    {
    //        return entities.SingleOrDefault(s => s.Id == id);
    //    }

    //    public void Insert(T entity)
    //    {
    //        if (entity == null)
    //        {
    //            throw new ArgumentNullException("entity");
    //        }

    //        entities.Add(entity);
    //        context.SaveChanges();
    //    }
    //    public void Update(T entity)
    //    {
    //        if (entity == null)
    //        {
    //            throw new ArgumentNullException("entity");
    //        }
    //        context.SaveChanges();

    //    }
    //    public void Delete(T entity)
    //    {
    //        if (entity == null)
    //        {
    //            throw new ArgumentNullException("entity");
    //        }
    //        entities.Remove(entity);
    //        context.SaveChanges();
    //    }
    //}
