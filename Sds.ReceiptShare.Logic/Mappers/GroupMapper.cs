using Entities = Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Logic.Models;
using Sds.ReceiptShare.Logic.Models.Group;
using System.Linq;

namespace Sds.ReceiptShare.Logic.Mappers
{
    internal static class GroupMapper
    {
        internal static GroupBasicDetails MapGroupBasicDetailsFromEntity<T>(Entities.Group entity)
        {
            return new GroupBasicDetails()
            {
                Created = entity.Created,
                Name = entity.Name,
                Id = entity.Id
            };
        }

        internal static GroupDetails MapGroupDetailsFromEntity(Entities.Group entity)
        {
            return new GroupDetails()
            {
                Created = entity.Created,
                Name = entity.Name,
                Id = entity.Id,
                Members = entity.Members.Select(s => MemberMapper.MapMemberDetailsFromEntity(s)).ToList(),
                PrimaryCurrency = CurrencyMapper.MapCurrencyFromEntity(entity.PrimaryCurrency),
                Purchases = entity.Purchases?.Select(s => PurchaseMapper.MapPurchaseDetailsFromEntity(s)).ToList()
            };
        }
    }
}
