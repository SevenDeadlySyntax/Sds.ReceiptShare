using System.Collections.Generic;

namespace Sds.ReceiptShare.Ui.Web.Models.Home
{
    public class HomeViewModel
    {
        public string UserName { get; set; }
        public List<GroupListItem> Groups { get; set; }
    }
}
