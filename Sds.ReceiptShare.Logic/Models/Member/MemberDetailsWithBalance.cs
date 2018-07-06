using System.Collections.Generic;

namespace Sds.ReceiptShare.Logic.Models.Member
{
    /// <summary>
    /// The member is a user of the app and a member of groups.
    /// </summary>
    public class MemberDetailsWithSummary : MemberDetails
    {
        public double TotalBenefit { get; set; }
        public double TotalContribution { get; set; }
    }
}
