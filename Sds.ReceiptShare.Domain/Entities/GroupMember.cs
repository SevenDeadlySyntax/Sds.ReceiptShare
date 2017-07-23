namespace Sds.ReceiptShare.Domain.Entities
{
    public class GroupMember : JoiningEntity
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
