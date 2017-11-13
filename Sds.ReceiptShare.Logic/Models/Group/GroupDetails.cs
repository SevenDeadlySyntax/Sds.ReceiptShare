using Sds.ReceiptShare.Logic.Models.Member;
using Sds.ReceiptShare.Logic.Models.Purchase;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Logic.Models.Group
{
    public class GroupDetails : GroupBasicDetails
    {
        public Currency PrimaryCurrency { get; set; }
        public ICollection<PurchaseDetails> Purchases { get; set; }
        public ICollection<Currency> Currencies { get; set; }
        public ICollection<MemberDetails> Members { get; set; }
    }
}
