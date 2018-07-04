using System;
using System.Collections.Generic;
using System.Linq;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class PurchaseBeneficiaries
    {
        public IEnumerable<PurchaseBeneficiary> Beneficiaries { get; set; }

        public PurchaseBeneficiaries(ICollection<string> names, double amount)
        {
            var share = amount / names.Count;
            Beneficiaries = names.Select(s => new PurchaseBeneficiary { Amount = share, Name = s });
        }
    }
}