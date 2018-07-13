using System.Collections.Generic;

namespace Sds.ReceiptShare.Domain.Entities
{
    public class Purchase : Entity
    {
        public string Description { get; set; }
        public double Amount { get; set; }

        public int CurrencyId { get; set; }
        public GroupCurrency Currency { get; set; }

        public string PurchaserId { get; set; }
        public ApplicationUser Purchaser { get; set; }

        public ICollection<PurchaseBeneficiary> Beneficiaries { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
