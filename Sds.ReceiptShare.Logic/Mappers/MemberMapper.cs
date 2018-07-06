using Entities = Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Logic.Models;
using Sds.ReceiptShare.Logic.Models.Member;

namespace Sds.ReceiptShare.Logic.Mappers
{
    internal static class MemberMapper
    {
        internal static T MapMemberDetailsFromEntity<T>(Entities.GroupMember entity, T result) where T : MemberDetails
        {

            result.Email = entity.Member.Email;
            result.Id = entity.Member.Id;
            result.ImageUrl = string.Empty; // TODO: Store this, update mapping
            result.Name = entity.Member.Name;
            result.IsAdministrator = entity.IsAdministrator;
            return result;
        }
    }
}
