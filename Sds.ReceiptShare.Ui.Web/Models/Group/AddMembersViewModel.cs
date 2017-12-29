using System.Collections.Generic;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class AddMembersViewModel
    {
        public string EmailAddresses { get; set; }
        public int GroupId { get; internal set; }
    }
}
