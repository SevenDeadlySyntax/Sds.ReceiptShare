using System;
using System.Collections.Generic;
using System.Text;

namespace Sds.ReceiptShare.Logic.Models
{
    public class RepaymentDetails
    {
        public string PayerId { get; set; }
        public string PayerName { get; set; }
        public string RecipientId { get; set; }
        public string RecipientName { get; set; }
        public double Value { get; set; }
    }
}
