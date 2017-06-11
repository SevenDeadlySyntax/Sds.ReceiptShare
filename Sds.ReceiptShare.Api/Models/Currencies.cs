using Sds.ReceiptShare.Domain.Models;

namespace Sds.ReceiptShare.Api.Models
{
    public class Currency
    {
        public string Name { get; set; }

        public string Symbol { get; set; }
        public Domain.Models.Currency Rate { get; internal set; }
        public int Id { get; internal set; }
    }
}