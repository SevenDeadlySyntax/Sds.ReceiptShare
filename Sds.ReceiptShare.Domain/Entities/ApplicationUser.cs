using Microsoft.AspNetCore.Identity;

namespace Sds.ReceiptShare.Domain.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
