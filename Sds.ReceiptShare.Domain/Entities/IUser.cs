using System;
using System.Collections.Generic;
using System.Text;

namespace Sds.ReceiptShare.Domain.Entities
{
    public interface IUser
    {
        string Id { get; set; }
        string Email { get; set; }
        ICollection<GroupMember> Groups { get; set; }
        string Name { get; }
    }


    public abstract class User
    {
        ICollection<GroupMember> Groups { get; set; }
        string Name { get; }
    }
}
