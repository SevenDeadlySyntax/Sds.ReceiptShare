using System;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Rate { get; internal set; }
    }
}