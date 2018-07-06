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

        int Add(GroupAdd group);
        void Update(GroupDetails group);

        IEnumerable<MemberDetails> GetMembers(int groupId);
        IEnumerable<MemberDetailsWithSummary> GetMembersWithSummary(int groupId);
        void RemoveMember(int id, int groupMemberId);
        void ManageMembers(int groupId, IEnumerable<string> emailAddresses);
        IEnumerable<RepaymentDetails> CalculateRepayments(int id);

        List<GroupCurrency> GetCurencies(int id, bool excludePrimary = false);
        void UpdateCurrencies(int id, IEnumerable<GroupCurrency> groupCurrencies);

        PurchaseDetails GetPurchase(int id, int purchaseId);
        void AddPurchase(PurchaseAddUpdate purchase);
        void UpdatePurchase(PurchaseAddUpdate purchase);
        IEnumerable<PurchaseDetails> GetPurchases(int id);
    }
}
