using Entities = Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Logic.Models;

namespace Sds.ReceiptShare.Logic.Mappers
{
    internal static class CurrencyMapper
    {
        internal static Currency MapCurrencyFromEntity(Entities.GroupCurrency entity)
        {
            var currency = MapCurrencyFromEntity(entity.Currency);
            currency.Rate = entity.ConvertionRate;
            return currency;
        }


        internal static Currency MapCurrencyFromEntity(Entities.Currency entity)
        {
            return new Currency()
            {
                Id = entity.Id,
                Name = entity.Name,
                Symbol = entity.Symbol,
                Rate = 1,
            };
        }
    }
}
