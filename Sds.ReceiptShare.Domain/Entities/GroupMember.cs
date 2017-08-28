namespace Sds.ReceiptShare.Domain.Entities
{
    public class GroupMember : JoiningEntity
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }

        public bool IsAdministrator { get; set; }
    }
}
