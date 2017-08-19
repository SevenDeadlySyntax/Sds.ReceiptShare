using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sds.ReceiptShare.Ui.Web.Models.Home
{
    public class GroupListItem
    {
        public string Name { get; set; }
        public int NumberOfMembers { get; set; }
        public DateTime Updated { get; set; }
    }
}
