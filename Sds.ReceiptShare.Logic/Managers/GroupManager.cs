using Sds.ReceiptShare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Data.Repository;

namespace Sds.ReceiptShare.Logic.Managers
{
    public class GroupManager : Manager, IGroupManager
    {
        public GroupManager(IRepository repository) : base(repository)
        {
        }

        public Group Add(Group group)
        {
            throw new NotImplementedException();
        }

        public void AddCurrency(int groupId, int currencyId)
        {
            throw new NotImplementedException();
        }

        public void AddMembers(int groupId, IEnumerable<GroupMember> members)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Just get the group's basic information. No linked objects
        /// </summary>
        /// <param name="id">The identifier for the group</param>
        /// <returns>Matched group or null</returns>
        public Group Get(int id)
        {
            return Repository.Read<Group>(id, "Administrator");
        }

        /// <summary>
        /// Just get the group's full details. No linked objects
        /// </summary>
        /// <param name="id">The identifier for the group</param>
        /// <returns>Matched group or null</returns>
        public Group GetDetails(int id)
        {
            return Repository.Read<Group>(id, "Members", "Administrator", "PrimaryCurrency", "GroupCurrencies", "GroupCurrencies.Currency");
        }

        public void RemoveMember(int groupId, int groupMemberId)
        {
            throw new NotImplementedException();
        }

        public ICollection<GroupMember> GetMembers(int groupId)
        {
            return Repository.Read<Group>(groupId, "Members", "Members.Member").Members;
        }
    }
}
