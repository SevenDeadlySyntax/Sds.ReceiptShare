using System;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Logic.Models.Group
{
    public class GroupAdd
    {
        public string Name { get; set; }        
        public int PrimaryCurrencyId { get; set; }
        public string CreatorId { get; set; }
    }
}
