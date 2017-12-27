using System;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Domain.Entities
{
    public class Group : DeletableEntity
    {
        public string Name { get; set; }

        public int PrimaryCurrencyId { get; set; }
        public Currency PrimaryCurrency { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public ICollection<GroupCurrency> GroupCurrencies { get; set; }
        public ICollection<GroupMember> Members { get; set; }
    }
}
