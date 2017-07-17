using System.Collections.Generic;

namespace Sds.ReceiptShare.Api.Models
{
    public class GroupMember
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public bool IsAdmin { get; internal set; }
    }
}
