using Sds.ReceiptShare.Logic.Interfaces;
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
using Sds.ReceiptShare.Core.ExtensionMethods;

namespace Sds.ReceiptShare.Logic.Managers
{
    public class GroupManager : Manager, IGroupManager
    {

        private readonly ApplicationUserManager _userManager;

        public GroupManager(IRepository repository, ApplicationUserManager userManager) : base(repository)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Just get the group's basic information. No linked objects
        /// </summary>
        /// <param name="id">The identifier for the group</param>
        /// <returns>Matched group or null</returns>
        public GroupBasicDetails Get(int id)
        {
            var entity = _repository.Read<Entities.Group>(id);
            return GroupMapper.MapGroupBasicDetailsFromEntity<GroupBasicDetails>(entity);
        }

        /// <summary>
        /// Just get the group's full details. No linked objects
        /// </summary>
        /// <param name="id">The identifier for the group</param>
        /// <returns>Matched group or null</returns>
        public GroupDetails GetDetails(int id)
        {
            var entity = _repository.Read<Entities.Group>(id, "Members", "Members.Member", "PrimaryCurrency", "GroupCurrencies", "GroupCurrencies.Currency");
            return GroupMapper.MapGroupDetailsFromEntity(entity);
        }

        // TODO: Probably this should be in a user manager
        public ICollection<GroupBasicDetails> GetUserGroups(string memberId)
        {
            var entities = _repository.ReadActive<Entities.Group>("Members").Where(s => s.Members.Select(x => x.MemberId).Contains(memberId));
            return entities.Select(s => GroupMapper.MapGroupBasicDetailsFromEntity<GroupBasicDetails>(s)).ToList();
        }

        /// <summary>
        /// Adds a new group with a primary currency and adds the creating user as an administrative group member
        /// </summary>
        /// <param name="group">Object containing details for new group</param>
        /// <returns></returns>
        public int Add(GroupAdd group)
        {
            var newGroup = new Entities.Group()
            {
                Created = DateTime.Now,
                Name = group.Name,
                PrimaryCurrencyId = group.PrimaryCurrencyId,
                GroupCurrencies = new List<Entities.GroupCurrency>() { new Entities.GroupCurrency { CurrencyId = group.PrimaryCurrencyId, ConvertionRate = 1 } }
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

            return newGroup.Id;
        }

        public void Update(GroupDetails group)
        {
            var entity = _repository.Read<Entities.Group>(group.Id);
            entity.Name = group.Name;
            entity.PrimaryCurrencyId = group.PrimaryCurrency.Id;

            _repository.Update(entity);
            _repository.Save();
        }

        public void UpdateCurrencies(int id, IEnumerable<GroupCurrency> groupCurrencies)
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
                    _repository.InsertManyToMany(new Entities.GroupCurrency { CurrencyId = item.Id, GroupId = id, ConvertionRate = item.Rate });
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

        public void ManageMembers(int groupId, IEnumerable<string> emailAddresses)
        {
            var members = ReadGroupMembers(groupId);

            // Add new group members
            // Check if user already exists and, if not, add one and email them with a registration link
            var addedMembers = emailAddresses.Where(s => !members.Select(x => x.Member.Email).Contains(s));
            foreach (var item in addedMembers)
            {
                var user = _userManager.FindByEmailAsync(item).Result;
                if (user == null)
                {
                    user = new Domain.Entities.ApplicationUser { UserName = item, Name = item, Email = item };
                    var createdUser = _userManager.CreateAsync(user);

                    // TODO: Email users if they are not already members
                }

                _repository.InsertManyToMany(new Entities.GroupMember { GroupId = groupId, MemberId = user.Id });
            }

            // Remove deteled group members
            var deletedMembers = members.Where(s => !s.IsAdministrator && !emailAddresses.Contains(s.Member.Email));
            foreach (var item in deletedMembers)
            {
                _repository.DeleteManyToMany<Entities.GroupMember>(item);
            }

            _repository.Save();
        }

        public List<GroupCurrency> GetCurencies(int id, bool excludePrimary = false)
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
            var entities = ReadGroupMembers(groupId);
            return entities.Select(s => MemberMapper.MapMemberDetailsFromEntity(s, new MemberDetails()));
        }

        private IEnumerable<Entities.GroupMember> ReadGroupMembers(int groupId)
        {
            return _repository.Read<Entities.Group>(groupId, "Members", "Members.Member").Members;
        }

        public PurchaseDetails GetPurchase(int id, int purchaseId)
        {
            var purchase = _repository.Read<Entities.Purchase>(purchaseId, "Currency", "Beneficiaries");

            if (purchase.GroupId != id) return null; // TODO: Throw exception?

            return PurchaseMapper.MapPurchaseDetailsFromEntity(purchase);
        }

        public void AddPurchase(PurchaseAddUpdate purchase)
        {
            var entity = PurchaseMapper.MapPurchaseAddUpdateToEntity(purchase);
            _repository.Insert(entity);
            _repository.Save();
        }

        public void UpdatePurchase(PurchaseAddUpdate purchase)
        {
            _repository.Update(PurchaseMapper.MapPurchaseAddUpdateToEntity(purchase));
            _repository.Save();
        }

        public IEnumerable<PurchaseDetails> GetPurchases(int id)
        {
            var entities = _repository.Read<Entities.Group>(id, "Purchases", "Purchases.Purchaser", "Purchases.Beneficiaries", "Purchases.Beneficiaries.Member", "Purchases.Currency", "Purchases.Currency.Currency").Purchases;
            return entities.Select(s => PurchaseMapper.MapPurchaseDetailsFromEntity(s));
        }

        public IEnumerable<MemberDetailsWithSummary> GetMembersWithSummary(int groupId)
        {
            var entities = ReadGroupMembers(groupId);
            var members = entities.Select(s => MemberMapper.MapMemberDetailsFromEntity(s, new MemberDetailsWithSummary())).ToList();
            var purchases = GetPurchases(groupId);

            // Could this be made more efficient?

            foreach (var item in members)
            {
                // Get all the purchases the user has benefitted from
                var benefitedPurchases = purchases.Where(s => s.Beneficiaries.Contains(item.Id)).ToList();

                // Devide the amount by the rate and split that by the number of beneficiaries.                
                item.TotalBenefit = benefitedPurchases.Select(s => (s.Amount / s.Currency.Rate) / s.Beneficiaries.Count()).Sum();

                // Get all the purchases the user has made
                var purchasesMade = purchases.Where(s => s.PurchaserId.Contains(item.Id)).ToList();

                // Transfer them into the group's primary currency and save the sum in the model
                item.TotalContribution = purchasesMade.Select(s => s.Amount / s.Currency.Rate).Sum();

            }

            return members;
        }

        public IEnumerable<RepaymentDetails> CalculateRepayments(int groupId)
        {
            var members = GetMembersWithSummary(groupId);
            var creditors = new Dictionary<string, double>();

            members.Where(s => s.TotalBenefit - s.TotalContribution < 0).OrderBy(s => s.TotalBenefit - s.TotalContribution).ToList().ForEach(s => creditors.Add(s.Id, Math.Abs(s.TotalBenefit - s.TotalContribution))); 
            var debtors = members.Where(s => s.TotalBenefit - s.TotalContribution > 0);

            var repayments = new List<RepaymentDetails>();
            foreach (var item in debtors)
            {
                var debt = item.TotalBenefit - item.TotalContribution;

                while (debt.Round() > 0 && creditors.Any(s => s.Value > 0))
                {
                    // Pick a creditor and decide how much to pay them
                    var creditor = creditors.First(s=> s.Value > 0);
                    var deduction = Math.Min(debt, creditor.Value);

                    // Update the creditor's credit and the debptor's debt to reflect this payment
                    creditors[creditor.Key] = creditor.Value - deduction;
                    debt -= deduction;

                    // Create new payment record
                    repayments.Add(new RepaymentDetails
                    {
                        PayerId = item.Id,
                        PayerName = item.Name,
                        Value = deduction,
                        RecipientId = creditor.Key,
                        RecipientName = members.First(s => s.Id == creditor.Key).Name
                    });
                };
            }

            return repayments;
        }
    }
}