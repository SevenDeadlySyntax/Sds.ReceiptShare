using Sds.ReceiptShare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Sds.ReceiptShare.Data.Repository;
using System.Linq.Expressions;
using System.Linq;
using Sds.ReceiptShare.Logic.Models.Lookup;

namespace Sds.ReceiptShare.Logic.Managers
{
    /// <summary>
    /// Used to managwe lookup types. Lookup types are used for populating lists. Linked entities will not be returned, as they are not expected to be included in lookup types.
    /// </summary>
    public class LookupManager : Manager, ILookupManager
    {
        public LookupManager(IRepository repository) : base(repository)
        {
        }

        public CurrencyLookupItem AddCurrency(CurrencyLookupItem currency)
        {
            var item = _repository.Insert(new Domain.Entities.Currency { Name = currency.Name, Symbol = currency.Symbol });
            _repository.Save();
            currency.Id = item.Id;
            return currency;
        }

        public List<CurrencyLookupItem> GetAllCurrencies()
        {
            var items = _repository.Read<Domain.Entities.Currency>();

            return items.Select(s => new CurrencyLookupItem() { Id = s.Id, Name = s.Name, Symbol = s.Symbol }).ToList();
        }
    }
}
