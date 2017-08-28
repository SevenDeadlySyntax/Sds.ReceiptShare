using Microsoft.EntityFrameworkCore;
using Sds.ReceiptShare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sds.ReceiptShare.Data.Repository
{
    public class ApplicationUserRepository : Repository, IApplicationUserRepository
    {
        private DataContext _context;

        public ApplicationUserRepository(DataContext context): base(context)
        {
            
        }

        public virtual IQueryable<T> ReadX<T>() where T : Entity
        {
            IQueryable<T> query = _context.Set<T>();
            return query;
        }

        public virtual IEnumerable<Group> GetGroups(string id)
        {
            return _context.Set<ApplicationUser>().Include("Groups, Groups.Group").SingleOrDefault(s=>s.Id == id)?.Groups.Select(s => s.Group);
        }
    }
}
 