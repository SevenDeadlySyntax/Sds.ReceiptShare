using System.Globalization;

namespace Sds.ReceiptShare.Domain.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string culture { get; set; }
    }
}