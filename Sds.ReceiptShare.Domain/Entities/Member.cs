using System.Collections.Generic;

namespace Sds.ReceiptShare.Domain.Entities
{
    public class Member : Entity
    {
        public string Name { get; set; }
        public ICollection<GroupMember> Groups { get; set; }
    }
}