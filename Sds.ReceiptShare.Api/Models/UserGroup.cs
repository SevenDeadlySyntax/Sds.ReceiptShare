using System.Collections.Generic;

namespace Sds.ReceiptShare.Api.Models
{
    public class UserGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }
        public Currency PrimaryCurrency { get; set; }
        public string AdminName { get; set; }
    }
}
