using System.Collections.Generic;

namespace Sds.ReceiptShare.Domain.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<GroupMember> Groups { get; set; }
    }
}