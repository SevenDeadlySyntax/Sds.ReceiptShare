using System;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Domain.Entities
{
    public class Group : DeletableEntity
    {
        public DateTime Created { get; set; }
        public Member Administrator { get; set; }
        public string Name { get; set; }
        public Currency PrimaryCurrency { get; set; }

        public ICollection<GroupCurrency> GroupCurrencies { get; set; }
        public ICollection<GroupMember> Members { get; set; }
    }
}
