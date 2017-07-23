namespace Sds.ReceiptShare.Domain.Entities
{
    public class GroupCurrency : JoiningEntity
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public double ConvertionRate { get; set; }
    }
}