using Entities = Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Logic.Models;
using Sds.ReceiptShare.Logic.Models.Purchase;
using System.Linq;

namespace Sds.ReceiptShare.Logic.Mappers
{
    internal static class PurchaseMapper
    {
        internal static PurchaseDetails MapPurchaseDetailsFromEntity(Entities.Purchase entity)
        {
            return new PurchaseDetails()
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Currency = CurrencyMapper.MapCurrencyFromEntity(entity.Currency),
                Description = entity.Description,
                PurchaserId = entity.PurchaserId,
                PurchaserName = entity.Purchaser?.Name,
                Date = entity.Created,
                Beneficiaries = entity.Beneficiaries.Select(s => s.MemberId).ToList()
            };
        }

        internal static Entities.Purchase MapPurchaseAddUpdateToEntity(PurchaseAddUpdate model)
        {
            var entity = new Entities.Purchase()
            {                
                Amount = model.Amount,
                CurrencyId = model.CurrencyId,
                Description = model.Description,
                PurchaserId = model.PurchaserId,
                Beneficiaries = model.Beneficiaries.Select(s=>new Entities.PurchaseBeneficiary { MemberId = s }).ToList(),
                GroupId = model.GroupId
            };

            if (model.Id != null) entity.Id = model.Id.Value;

            return entity;
        }
    }
}
