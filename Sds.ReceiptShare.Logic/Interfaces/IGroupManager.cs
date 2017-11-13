using Sds.ReceiptShare.Logic.Models;
using Sds.ReceiptShare.Logic.Models.Group;
using Sds.ReceiptShare.Logic.Models.Member;
using Sds.ReceiptShare.Logic.Models.Purchase;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Logic.Interfaces
{
    public interface IGroupManager
    {
        GroupBasicDetails Get(int id);
        GroupDetails GetDetails(int id);
        ICollection<GroupBasicDetails> GetUserGroups(string userId); 

        GroupAdd Add(GroupAdd group);
        void Update(GroupDetails group);

        IEnumerable<MemberDetails> GetMembers(int groupId);
        void RemoveMember(int id, int groupMemberId);
        void AddMembers(int groupId, IEnumerable<string> groupMembers);

        List<Currency> GetCurencies(int id, bool excludePrimary = false);
        void UpdateCurrencies(int id, IEnumerable<Currency> groupCurrencies);

        PurchaseDetails GetPurchase(int id, int purchaseId);
        void AddPurchase(PurchaseAddUpdate purchase);
        void UpdatePurchase(PurchaseAddUpdate purchase);
        IEnumerable<PurchaseDetails> GetPurchases(int id);
    }
}
