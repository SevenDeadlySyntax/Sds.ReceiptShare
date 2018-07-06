namespace Sds.ReceiptShare.Ui.Web.Models.Group
{
    public class Member
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public double Balance { get { return TotalContribution - TotalBenefit; } }
        public bool IsAdministrator { get; set; }
        public double TotalBenefit { get; internal set; }
        public double TotalContribution { get; internal set; }
    }
}