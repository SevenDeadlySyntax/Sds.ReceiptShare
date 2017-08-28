using Sds.ReceiptShare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sds.ReceiptShare.Data.Repository
{
    public interface IApplicationUserRepository : IRepository
    {
        IEnumerable<Group> GetGroups(string id);
    }
}
