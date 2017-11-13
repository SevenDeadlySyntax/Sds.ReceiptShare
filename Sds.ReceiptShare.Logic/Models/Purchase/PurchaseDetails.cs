using System.Collections.Generic;

namespace Sds.ReceiptShare.Logic.Models.Purchase
{
    public class PurchaseDetails
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public string PurchaserId { get; set; }
        public ICollection<string> Beneficiaries { get; set; }
    }
}
