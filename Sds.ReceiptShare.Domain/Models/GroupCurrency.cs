namespace Sds.ReceiptShare.Domain.Models
{
    public class GroupCurrency
    {
        public int Id { get; set; }
        public Currency Currency { get; set; }
        public double ConvertionRate { get; set; }
    }
}