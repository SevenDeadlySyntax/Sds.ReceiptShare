using System.Collections.Generic;

namespace Sds.ReceiptShare.Domain.Entities
{
    /// <summary>
    /// The member is a user of the app and a member of groups.
    /// </summary>
    public class Member : Entity
    {
        public string Name { get; set; }
        public ICollection<GroupMember> Groups { get; set; }

        /// <summary>
        /// Rather than adding the member properties to the actual application user, I have decided to create a 
        /// "member" as a separate entity, which "has" an appication user. This makes things a little tidier with regards to the use
        /// of the repository pattern, but does mean that an application user could get created without having a linked member... still not sure
        /// what the best solution is here...
        /// This 
        /// </summary>
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}