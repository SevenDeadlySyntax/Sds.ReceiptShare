using System;
using System.Collections.Generic;
using System.Text;

namespace Sds.ReceiptShare.Core.ExtensionMethods
{
    public static class RoundedDouble
    {
        public static double Round(this double value, int decimals = 2)
        {
            return Math.Round(value, decimals);
        }
    }
}
