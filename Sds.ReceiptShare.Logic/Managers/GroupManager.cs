using Sds.ReceiptShare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Data.Repository;
using System.Linq.Expressions;
using System.Linq;

namespace Sds.ReceiptShare.Logic.Managers
{
    public class GroupManager : Manager, IGroupManager
    {
        public GroupManager(IRepository repository) : base(repository)
        {
        }

        public Group Add(Group group)
        {
            _repository.Insert<Group>(group);
            _repository.Save();
            return group;
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
            return _repository.Read<Group>(id);
        }

        /// <summary>
        /// Just get the group's full details. No linked objects
        /// </summary>
        /// <param name="id">The identifier for the group</param>
        /// <returns>Matched group or null</returns>
        public Group GetDetails(int id)
        {
            return _repository.Read<Group>(id, "Members", "PrimaryCurrency", "GroupCurrencies", "GroupCurrencies.Currency");
        }

        public void RemoveMember(int groupId, int groupMemberId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupMember> GetMembers(int groupId)
        {
            return _repository.Read<Group>(groupId, "Members", "Members.Member").Members;
        }

        public ICollection<Group> GetUserGroups(string memberId)
        {
            return _repository.ReadActive<Group>("Members").Where(s => s.Members.Select(x => x.MemberId).Contains(memberId)).ToList();
        }
    }
}
