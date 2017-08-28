using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class CreateViewModel
    {
        public string Name { get; set; }
        public List<string> UserIds { get; set; }
    }
}
