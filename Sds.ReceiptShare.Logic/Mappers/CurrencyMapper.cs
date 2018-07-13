using Entities = Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Logic.Models;

namespace Sds.ReceiptShare.Logic.Mappers
{
    internal static class CurrencyMapper
    {
        internal static GroupCurrency MapCurrencyFromEntity(Entities.GroupCurrency entity)
        {
            var currency = MapCurrencyFromEntity(entity.Currency);
            currency.Rate = entity.ConvertionRate;
            return currency;
        }

        internal static GroupCurrency MapCurrencyFromEntity(Entities.Currency entity, double conversionRate)
        {
            var currency = MapCurrencyFromEntity(entity);
            currency.Rate = conversionRate;
            return currency;
        }

        internal static GroupCurrency MapCurrencyFromEntity(Entities.Currency entity)
        {
            return new GroupCurrency()
            {
                Id = entity.Id,
                Name = entity.Name,
                Symbol = entity.Symbol,
                Rate = 1,
            };
        }
    }
}
