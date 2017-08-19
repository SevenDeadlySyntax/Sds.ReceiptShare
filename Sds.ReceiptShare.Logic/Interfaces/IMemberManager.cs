using Sds.ReceiptShare.Domain.Entities;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Logic.Interfaces
{
    public interface IMemberManager
    {
        ICollection<Group> GetGroups(int id);
    }
}