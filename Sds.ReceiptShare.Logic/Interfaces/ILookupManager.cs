using System.Collections.Generic;
using Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Logic.Models.Lookup;

namespace Sds.ReceiptShare.Logic.Interfaces
{
    public interface ILookupManager
    {
        CurrencyLookupItem AddCurrency(CurrencyLookupItem currency);
        List<CurrencyLookupItem> GetAllCurrencies();
    }
}