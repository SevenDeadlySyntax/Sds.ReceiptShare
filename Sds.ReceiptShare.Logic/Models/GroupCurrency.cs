using System.Globalization;

namespace Sds.ReceiptShare.Logic.Models
{
    public class GroupCurrency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Rate { get; set; }
    }
}