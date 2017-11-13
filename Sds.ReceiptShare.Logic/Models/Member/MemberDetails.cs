using System.Collections.Generic;

namespace Sds.ReceiptShare.Logic.Models.Member
{
    /// <summary>
    /// The member is a user of the app and a member of groups.
    /// </summary>
    public class MemberDetails
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}