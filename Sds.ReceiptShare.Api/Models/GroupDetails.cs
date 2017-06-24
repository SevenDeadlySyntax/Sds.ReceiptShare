using System.Collections.Generic;

namespace Sds.ReceiptShare.Api.Models
{
    public class GroupDetails : Group
    {
        public IEnumerable<Currency> Currencies { get; set; }
        public Currency PrimaryCurrency { get; set; }
        public string AdminName { get; set; }
    }
}
