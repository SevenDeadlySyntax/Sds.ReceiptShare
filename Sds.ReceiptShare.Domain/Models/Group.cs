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
        public IEnumerable<GroupCurrency> PurchaseCurrencies { get; set; }
        public Currency PrimaryCurrency { get; set; }
        public IEnumerable<Member> Members { get; set; }
    }
}
