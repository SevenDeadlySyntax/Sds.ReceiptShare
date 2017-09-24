﻿using System;

namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class Purchase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Currency Currency { get; set; }
        public double Value { get; set; }
        public string PurchasedBy { get; set; }
        public DateTime Date { get; set; }
    }
}