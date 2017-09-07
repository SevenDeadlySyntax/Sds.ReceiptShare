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
        List<GroupCurrency> GetCurencies(int id);
        void AddMembers(int groupId, IEnumerable<GroupMember> groupMembers);
        Group Add(Group group);
        void Update(Group group);
        void RemoveMember(int groupId, int groupMemberId);
        void AddCurrencies(int groupId, IEnumerable<GroupCurrency> groupCurrencies);
    }
}
