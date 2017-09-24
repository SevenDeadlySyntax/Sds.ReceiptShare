namespace Sds.ReceiptShare.Domain.Entities
{
    public class PurchaseBeneficiary
    {
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
        
        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }
    }
}