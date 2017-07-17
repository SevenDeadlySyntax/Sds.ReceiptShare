namespace Sds.ReceiptShare.Domain.Models
{
    public class GroupCurrency
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public double ConvertionRate { get; set; }
    }
}