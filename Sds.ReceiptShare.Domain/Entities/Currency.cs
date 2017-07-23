using System.Globalization;

namespace Sds.ReceiptShare.Domain.Entities
{
    public class Currency : Entity
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}