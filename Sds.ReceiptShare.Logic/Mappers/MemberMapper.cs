using Entities = Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Logic.Models;
using Sds.ReceiptShare.Logic.Models.Member;

namespace Sds.ReceiptShare.Logic.Mappers
{
    internal static class MemberMapper
    {
        internal static MemberDetails MapMemberDetailsFromEntity(Entities.GroupMember entity)
        {
            return new MemberDetails()
            {
                Email = entity.Member.Email,
                Id = entity.Member.Id,
                ImageUrl = string.Empty, // TODO: Store this, update mapping
                Name = entity.Member.Name
            };
        }
    }
}
