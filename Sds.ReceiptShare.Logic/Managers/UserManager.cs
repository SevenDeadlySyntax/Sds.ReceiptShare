using Sds.ReceiptShare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Data.Repository;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Sds.ReceiptShare.Logic.Managers
{
    public class UserManager : UserManager<ApplicationUser>
    {
        private IRepository _repository;
        
    }
}
