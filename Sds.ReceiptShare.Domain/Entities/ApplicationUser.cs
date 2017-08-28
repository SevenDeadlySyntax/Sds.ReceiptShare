using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Domain.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //public int MemberId { get; set; }
        //public Member Member { get; set; }
        public string Name { get; set; }

        public ICollection<GroupMember> Groups { get; set; }
    }
}
