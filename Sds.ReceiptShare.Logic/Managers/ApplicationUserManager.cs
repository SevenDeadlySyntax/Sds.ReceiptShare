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

namespace Sds.ReceiptShare.Logic.Managers
{
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
            return _repository.GetGroups(user.Id);
        }
    }
}
