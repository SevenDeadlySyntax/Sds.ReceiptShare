using System;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Domain.Models
{
    public class Party
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Member Administrator { get; set; }
        public string Name { get; set; }
        public IEnumerable<PartyCurrency> PurchaseCurrencies { get; set; }
        public Currency PrimaryCurrency { get; set; }
        public IEnumerable<Member> Members { get; set; }
    }
}
