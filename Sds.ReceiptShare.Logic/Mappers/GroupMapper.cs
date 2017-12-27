using Entities = Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Logic.Models;
using Sds.ReceiptShare.Logic.Models.Group;
using System.Linq;

namespace Sds.ReceiptShare.Logic.Mappers
{
    internal static class GroupMapper
    {
        internal static T MapGroupBasicDetailsFromEntity<T>(Entities.Group entity) where T : GroupBasicDetails
        {
            return (T)new GroupBasicDetails
            {
                Created = entity.Created,
                Name = entity.Name,
                Id = entity.Id
            };
        }

        internal static GroupDetails MapGroupDetailsFromEntity(Entities.Group entity)
        {
            var details = (GroupDetails)MapGroupBasicDetailsFromEntity<GroupDetails>(entity);

            details.Members = entity.Members.Select(s => MemberMapper.MapMemberDetailsFromEntity(s)).ToList();
            details.PrimaryCurrency = CurrencyMapper.MapCurrencyFromEntity(entity.PrimaryCurrency);
            details.Purchases = entity.Purchases.Select(s => PurchaseMapper.MapPurchaseDetailsFromEntity(s)).ToList();
            return details; 
        }
    }
}
