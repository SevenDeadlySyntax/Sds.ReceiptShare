using Sds.ReceiptShare.Domain.Entities;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Logic.Interfaces
{
    public interface IGroupManager
    {
        Group Get(int id);
        ICollection<Group> GetUserGroups(string userId); 
        Group GetDetails(int id);
        IEnumerable<GroupMember> GetMembers(int groupId);
        Group Add(Group group);
        void AddMembers(int groupId, IEnumerable<GroupMember> members);
        void RemoveMember(int groupId, int groupMemberId);
        void AddCurrency(int groupId, int currencyId);

    }
}
