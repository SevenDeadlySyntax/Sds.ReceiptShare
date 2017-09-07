using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class CurrencyCheckboxListItem
    {
        public Currency Currency { get; set; }
        public double Rate { get; set; }
        public bool Checked { get; set; }
    }
}
