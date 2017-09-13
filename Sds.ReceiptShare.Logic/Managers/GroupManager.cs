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

        public void UpdateCurrencies(int id, IEnumerable<GroupCurrency> groupCurrencies)
        {
            var existingCurrencies = _repository.Read<Group>(id, "PrimaryCurrency", "GroupCurrencies", "GroupCurrencies.Currency").GroupCurrencies.ToList();
            var entitiesToAdd = groupCurrencies.Where(s => !existingCurrencies.Select(x => x.CurrencyId).Contains(s.CurrencyId)).ToList();
            var entitiesToRemove = existingCurrencies.Where(s => !groupCurrencies.Select(x => x.CurrencyId).Contains(s.CurrencyId));

            // Add all the passed in items that don't already exist
            if (entitiesToAdd.Any())
            {
                foreach (var item in entitiesToAdd)
                {
                    _repository.InsertManyToMany<GroupCurrency>(item);
                }
                _repository.Save();
            }

            // Remove any that are not in the list 
            if (entitiesToRemove.Any())
            {
                foreach (var item in entitiesToRemove)
                {
                    // TODO: don't remove them if they are associated with purchases
                    _repository.DeleteManyToMany<GroupCurrency>(item);
                }
                _repository.Save();
            }

            // Update any chnages to the rate
            foreach (var item in groupCurrencies)
            {
                if (existingCurrencies.Any(s => s.CurrencyId == item.CurrencyId && s.ConvertionRate != item.ConvertionRate))
                {
                    _repository.UpdateManyToMany<GroupCurrency>(item);
                }
            }
            _repository.Save();
        }

        public void Update(Group group)
        {
            _repository.Update<Group>(group);
            _repository.Save();
        }

        public void AddMembers(int groupId, IEnumerable<GroupMember> groupMembers)
        {
            foreach (var item in groupMembers)
            {
                _repository.InsertManyToMany<GroupMember>(item);
            }

            _repository.Save();
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

        public List<GroupCurrency> GetCurencies(int id, bool excludePrimary = false)
        {
            return _repository.Read<Group>(id, "PrimaryCurrency", "GroupCurrencies", "GroupCurrencies.Currency").GroupCurrencies.Where(s => !excludePrimary || s.CurrencyId != s.Group.PrimaryCurrency.Id).ToList();
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