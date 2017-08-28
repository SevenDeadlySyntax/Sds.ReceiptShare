using Sds.ReceiptShare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Data.Repository;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sds.ReceiptShare.Logic.Managers
{
    public class MemberManager : IMemberManager
    {
        private IMemberRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberManager(IMemberRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
        }
        
        public ICollection<Group> GetGroups(int id)
        {
            return _repository.Read<Member>(id, "Groups", "Groups.Group").Groups.Select(s=> s.Group).ToList();
        }

        public Member Get(string userId)
        {
            return _repository.GetMemberFromUserId(userId);
        }
    }
}
