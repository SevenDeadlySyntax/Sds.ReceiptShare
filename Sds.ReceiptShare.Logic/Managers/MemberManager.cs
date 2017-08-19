using Sds.ReceiptShare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Data.Repository;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Sds.ReceiptShare.Logic.Managers
{
    public class MemberManager : IMemberManager
    {
        private IRepository _repository;

        public MemberManager(IRepository repository)
        {
            _repository = repository;
        }
        
        public ICollection<Group> GetGroups(int id)
        {
            return _repository.Read<Member>(id, "Groups", "Groups.Group").Groups.Select(s=> s.Group).ToList();
        }
    }
}
