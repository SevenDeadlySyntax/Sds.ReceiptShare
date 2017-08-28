using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class DetailsViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<GroupMember> Members { get; set; }
    }
}
