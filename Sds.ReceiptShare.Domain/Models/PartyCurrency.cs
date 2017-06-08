namespace Sds.ReceiptShare.Domain.Models
{
    public class PartyCurrency
    {
        public int Id { get; set; }
        public Currency Currency { get; set; }
        public double ConvertionRate { get; set; }
    }
}