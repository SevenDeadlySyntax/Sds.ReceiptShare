﻿using System;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class DetailsViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<Member> Members { get; set; }
        public List<Purchase> Purchases { get; set; }
        public IEnumerable<Currency> Currencies { get; internal set; }
    }
}