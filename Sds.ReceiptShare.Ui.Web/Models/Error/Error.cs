using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sds.ReceiptShare.Ui.Web.Models.Error
{
    public class Error
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Description { get; set; }       
    }
}
