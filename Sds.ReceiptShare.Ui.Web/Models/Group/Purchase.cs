using System;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class Purchase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public double Value { get; set; }
        public string PurchasedBy { get; set; }
    }
}