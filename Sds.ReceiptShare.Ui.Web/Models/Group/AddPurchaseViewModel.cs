using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class AddPurchaseViewModel
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public int Currency { get; set; }
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public CheckboxList Beneficiaries { get; set; }
        public string Purchaser { get; internal set; }
        public IEnumerable<SelectListItem> Members { get; set; }
    }
}
