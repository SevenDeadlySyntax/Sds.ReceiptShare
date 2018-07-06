using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class Repayment
    {
        public string Payer { get; internal set; }
        public string Recipient { get; internal set; }
        public double Value { get; internal set; }
    }
}
