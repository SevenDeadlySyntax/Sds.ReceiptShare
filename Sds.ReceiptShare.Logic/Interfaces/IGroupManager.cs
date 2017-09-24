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
        List<GroupCurrency> GetCurencies(int id, bool excludePrimary = false);
        void AddMembers(int groupId, IEnumerable<GroupMember> groupMembers);
        Group Add(Group group);
        void Update(Group group);
        void RemoveMember(int id, int groupMemberId);
        void UpdateCurrencies(int id, IEnumerable<GroupCurrency> groupCurrencies);

        Purchase GetPurchase(int id, int purchaseId);
        Purchase AddPurchase(Purchase purchase);
        Purchase UpdatePurchase(Purchase purchase);
        IEnumerable<Purchase> GetPurchases(int id);
    }
}
