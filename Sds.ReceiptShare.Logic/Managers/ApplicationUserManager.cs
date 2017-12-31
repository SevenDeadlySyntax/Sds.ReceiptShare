using Sds.ReceiptShare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Data.Repository;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Sds.ReceiptShare.Logic.Managers
{
    //TODO: Move all code from user controller into here and refactor controller.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly IRepository _repository; 

        public ApplicationUserManager(IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger,
            IRepository repository) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _repository = repository;
        }

        public IEnumerable<GroupMember> GetGroups(ApplicationUser user){
            return GetGroups(user.Id);
        }

        public IEnumerable<GroupMember> GetGroups(string id)
        {
            return _repository.GetGroups(id);
        }

        public bool IsAdministrator(string id, int groupId)
        {
            return GetGroups(id).Any(s=> s.GroupId == groupId && s.IsAdministrator);
        }

        public bool IsInGroup(string id, int groupId)
        {
            return _repository.GetGroups(id).Any(s => s.GroupId == groupId);
        }
    }
}
