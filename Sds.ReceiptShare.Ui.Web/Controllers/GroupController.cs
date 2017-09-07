using Microsoft.AspNetCore.Mvc;
using Sds.ReceiptShare.Ui.Web.Models.Group;
using Sds.ReceiptShare.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Sds.ReceiptShare.Logic.Managers;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Sds.ReceiptShare.Ui.Web.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IGroupManager _groupManager;
        private readonly ApplicationUserManager _userManager;
        private readonly ILookupManager _lookupManager;

        public GroupController(IGroupManager groupManager, ApplicationUserManager userManager, ILookupManager lookupManager)
        {
            _groupManager = groupManager;
            _userManager = userManager;
            _lookupManager = lookupManager;
        }

        public IActionResult Index(int id)
        {
            var group = _groupManager.Get(id);
            var members = _groupManager.GetMembers(id);
            var currencies = _groupManager.GetCurencies(id);
            var model = new DetailsViewModel()
            {
                Id = group.Id,
                Name = group.Name,
                CreatedOn = group.Created,
                Members = members?.Select(s => new Member { Name = s.Member.Name, UserId = s.Member.Id, IsAdministrator = s.IsAdministrator }).ToList(),
                Currencies = currencies?.Select(s => new Currency { Id = s.CurrencyId, Name = s.Currency.Name, Symbol = s.Currency.Symbol, Rate = s.ConvertionRate })
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View(new CreateViewModel());
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel model)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var group = new Domain.Entities.Group()
            {
                Created = DateTime.UtcNow,
                Name = model.Name,
                Members = new List<Domain.Entities.GroupMember> { new Domain.Entities.GroupMember() { IsAdministrator = true, Member = user } }
            };

            _groupManager.Add(group);
            return RedirectToAction("Index", new { id = group.Id });
        }

        [HttpGet]
        public IActionResult AddMembers(int id)
        {
            return View(new AddMembersViewModel()
            {
                GroupId = id,
            });
        }

        [HttpPost()]
        public IActionResult AddMembers(int id, AddMembersViewModel model)
        {
            //var group = _groupManager.Get(id);
            var groupMembers = new List<Domain.Entities.GroupMember>();

            foreach (var item in model.EmailAddresses.Split(","))
            {
                var user = _userManager.FindByEmailAsync(item).Result;
                if (user == null)
                {
                    user = new Domain.Entities.ApplicationUser { UserName = item, Name = item, Email = item };
                    var createdUser = _userManager.CreateAsync(user);
                }

                groupMembers.Add(new Domain.Entities.GroupMember { GroupId = id, Member = user });
            }

            _groupManager.AddMembers(id, groupMembers);
            return RedirectToAction("Index", new { id = id });
        }

        [HttpGet]
        public IActionResult AddCurrencies(int id)
        {
            var currencies = _lookupManager.GetAll<Domain.Entities.Currency>();
            var selectedCurrencies = _groupManager.GetCurencies(id);
            var checkboxList = new List<CurrencyCheckboxListItem>();

            foreach (var item in currencies)
            {
                var selectedCurrency = selectedCurrencies.SingleOrDefault(s => s.CurrencyId == item.Id);
                checkboxList.Add(new CurrencyCheckboxListItem()
                {
                    Checked = selectedCurrency != null,
                    Rate = selectedCurrency?.ConvertionRate ?? 0,
                    Currency = new Currency { Id = item.Id, Name = item.Name, Symbol = item.Symbol, }
                });
            }

            var model = new AddCurrenciesViewModel()
            {
                GroupId = id,
                CurrencyCheckboxList = checkboxList
            };

            return View(model);
        }

        [HttpPost()]
        public IActionResult AddCurrencies(int id, AddCurrenciesViewModel model)
        {
            _groupManager.AddCurrencies(id, model.CurrencyCheckboxList.Where(S=> S.Checked).Select(s =>
                new Domain.Entities.GroupCurrency()
                {
                    CurrencyId = s.Currency.Id,
                    GroupId = id,
                    ConvertionRate = s.Rate
                }));

            return RedirectToAction("Index", new { id = id });
        }

    }
}