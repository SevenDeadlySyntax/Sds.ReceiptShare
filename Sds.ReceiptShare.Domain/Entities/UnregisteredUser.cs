using System;
using System.Collections.Generic;
using System.Text;

namespace Sds.ReceiptShare.Domain.Entities
{
    public class UnregisteredUser : IUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public ICollection<GroupMember> Groups { get; set; }
                
        public string Name {
            get {
                return this.Email;
            }             
        }
    }
}
