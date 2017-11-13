﻿using Sds.ReceiptShare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using Entities = Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Data.Repository;
using System.Linq;
using Sds.ReceiptShare.Logic.Models.Group;
using Sds.ReceiptShare.Logic.Models.Member;
using Sds.ReceiptShare.Logic.Models;
using Sds.ReceiptShare.Logic.Mappers;
using Sds.ReceiptShare.Logic.Models.Purchase;

namespace Sds.ReceiptShare.Logic.Managers
{
    public class GroupManager : Manager, IGroupManager
    {
        public GroupManager(IRepository repository) : base(repository)
        {
        }


        /// <summary>
        /// Just get the group's basic information. No linked objects
        /// </summary>
        /// <param name="id">The identifier for the group</param>
        /// <returns>Matched group or null</returns>
        public GroupBasicDetails Get(int id)
        {
            var entity = _repository.Read<Entities.Group>(id);
            return GroupMapper.MapGroupBasicDetailsFromEntity(entity);
        }

        /// <summary>
        /// Just get the group's full details. No linked objects
        /// </summary>
        /// <param name="id">The identifier for the group</param>
        /// <returns>Matched group or null</returns>
        public GroupDetails GetDetails(int id)
        {
            var entity = _repository.Read<Entities.Group>(id, "Members", "PrimaryCurrency", "GroupCurrencies", "GroupCurrencies.Currency");
            return GroupMapper.MapGroupDetailsFromEntity(entity);
        }
        
        // TODO: Probably this should be in a user manager
        public ICollection<GroupBasicDetails> GetUserGroups(string memberId)
        {
            var entities = _repository.ReadActive<Entities.Group>("Members").Where(s => s.Members.Select(x => x.MemberId).Contains(memberId));
            return entities.Select(s => GroupMapper.MapGroupBasicDetailsFromEntity(s)).ToList();
        }

        /// <summary>
        /// Adds a new group with a primary currency and adds the creating user as an administrative group member
        /// </summary>
        /// <param name="group">Object containing details for new group</param>
        /// <returns></returns>
        public GroupAdd Add(GroupAdd group)
        {
            var newGroup = new Entities.Group()
            {
                Created = DateTime.Now,
                Name = group.Name,
                PrimaryCurrencyId = group.PrimaryCurrencyId
            };

            _repository.Insert(newGroup);
            _repository.Save();

            var groupMember = new Entities.GroupMember()
            {
                GroupId = newGroup.Id,
                MemberId = group.CreatorId,                
                IsAdministrator = true,
            };

            _repository.InsertManyToMany(groupMember);
            _repository.Save();

            return group;
        }

        public void Update(GroupDetails group)
        {
            var entity = _repository.Read<Entities.Group>(group.Id);
            entity.Name = group.Name;
            entity.PrimaryCurrencyId = group.PrimaryCurrency.Id;

            _repository.Update(entity);
            _repository.Save();
        }

        public void UpdateCurrencies(int id, IEnumerable<Currency> groupCurrencies)
        {
            // TODO: check for admin
            var existingCurrencies = _repository.Read<Entities.Group>(id, "PrimaryCurrency", "GroupCurrencies", "GroupCurrencies.Currency").GroupCurrencies.ToList();
            var entitiesToAdd = groupCurrencies.Where(s => !existingCurrencies.Select(x => x.CurrencyId).Contains(s.Id)).ToList();
            var entitiesToRemove = existingCurrencies.Where(s => !groupCurrencies.Select(x => x.Id).Contains(s.CurrencyId));

            // Add all the passed in items that don't already exist
            if (entitiesToAdd.Any())
            {
                foreach (var item in entitiesToAdd)
                {
                    _repository.InsertManyToMany(new Entities.GroupCurrency { CurrencyId = item.Id, GroupId = id, ConvertionRate = item.Rate});
                }

                _repository.Save();
            }

            // Remove any that are not in the list 
            if (entitiesToRemove.Any())
            {
                foreach (var item in entitiesToRemove)
                {
                    // TODO: don't remove them if they are associated with purchases
                    _repository.DeleteManyToMany(item);
                }
                _repository.Save();
            }

            // Update any changes to the rate
            foreach (var item in existingCurrencies)
            {
                if (groupCurrencies.Any(s => s.Id == item.CurrencyId && s.Rate != item.ConvertionRate))
                {
                    _repository.UpdateManyToMany(item);
                }
            }
            _repository.Save();
        }

        public void AddMembers(int groupId, IEnumerable<string> groupMemberIds)
        {
            foreach (var item in groupMemberIds)
            {
                _repository.InsertManyToMany(new Entities.GroupMember { GroupId = groupId, MemberId = item});
            }

            _repository.Save();
        }

        public List<Currency> GetCurencies(int id, bool excludePrimary = false)
        {

            var entities = _repository.Read<Entities.Group>(id, "PrimaryCurrency", "GroupCurrencies", "GroupCurrencies.Currency").GroupCurrencies.Where(s => !excludePrimary || s.CurrencyId != s.Group.PrimaryCurrency.Id);
            return entities.Select(s => CurrencyMapper.MapCurrencyFromEntity(s)).ToList();
        }

        public void RemoveMember(int groupId, int groupMemberId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MemberDetails> GetMembers(int groupId)
        {
            var entities = _repository.Read<Entities.Group>(groupId, "Members", "Members.Member").Members;
            return entities.Select(s => MemberMapper.MapMemberDetailsFromEntity(s));
        }

        public PurchaseDetails GetPurchase(int id, int purchaseId)
        {
            var purchase = _repository.Read<Entities.Purchase>(purchaseId, "Currency", "Beneficiaries");

            if (purchase.GroupId != id) return null; // TODO: Throw exception?

            return PurchaseMapper.MapPurchaseDetailsFromEntity(purchase);
        }

        public void AddPurchase(PurchaseAddUpdate purchase)
        {
            _repository.Insert(PurchaseMapper.MapPurchaseAddUpdateToEntity(purchase));
            _repository.Save();
        }

        public void UpdatePurchase(PurchaseAddUpdate purchase)
        {
            _repository.Update(PurchaseMapper.MapPurchaseAddUpdateToEntity(purchase));
            _repository.Save();
        }

        public IEnumerable<PurchaseDetails> GetPurchases(int id)
        {
            var entities = _repository.Read<Entities.Group>(id, "Purchases", "Purchases.Purchaser", "Purchases.Beneficiaries", "Purchases.Beneficiaries.Member", "Purchases.Currency").Purchases;
            return entities.Select(s => PurchaseMapper.MapPurchaseDetailsFromEntity(s));
        }
    }
}