using System.Collections.Generic;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class AddCurrenciesViewModel
    {
        public int GroupId { get; set; }
        public List<CurrencyCheckboxListItem> CurrencyCheckboxList { get; set; }
    }
}
