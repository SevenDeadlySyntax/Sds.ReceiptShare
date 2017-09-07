using System.Collections.Generic;

namespace Sds.ReceiptShare.Domain.Entities
{
    public class Purchase : Entity
    {
        public string Description { get; set; }
        public double Amount { get; set; }
        public GroupCurrency Currency { get; set; }
        public ApplicationUser Purchaser { get; set; }
        public IEnumerable<ApplicationUser> Beneficiaries { get; set; }
    }
}
