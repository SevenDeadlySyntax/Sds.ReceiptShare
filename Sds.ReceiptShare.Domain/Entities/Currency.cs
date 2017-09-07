using System.Globalization;

namespace Sds.ReceiptShare.Domain.Entities
{
    public class Currency : LookupEntity
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}