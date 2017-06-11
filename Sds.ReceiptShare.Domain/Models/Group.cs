using System;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Domain.Models
{
    public class Group
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Member Administrator { get; set; }
        public string Name { get; set; }
        public ICollection<GroupCurrency> PurchaseCurrencies { get; set; }
        public Currency PrimaryCurrency { get; set; }
        public ICollection<GroupMember> Members { get; set; }
    }

    public class GroupMember
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
