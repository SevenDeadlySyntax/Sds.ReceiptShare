using System;
using System.Collections.Generic;
using System.Text;

namespace Sds.ReceiptShare.Domain.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public GroupCurrency Currency { get; set; }
        public Member Purchaser { get; set; }
        public IEnumerable<Member> Beneficiaries { get; set; }
    }
}
