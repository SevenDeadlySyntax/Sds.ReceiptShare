using Microsoft.AspNetCore.Mvc;
using Sds.ReceiptShare.Ui.Web.Models.Group;
using Sds.ReceiptShare.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Sds.ReceiptShare.Logic.Managers;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sds.ReceiptShare.Logic.Models.Group;
using Sds.ReceiptShare.Logic.Models;

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
            var group = _groupManager.GetDetails(id);
            var members = _groupManager.GetMembers(id);
            var purchases = _groupManager.GetPurchases(id);
            var currencies = _groupManager.GetCurencies(id, true);
            var model = new DetailsViewModel()
            {
                Id = group.Id,
                Name = group.Name,
                CreatedOn = group.Created,
                Members = members?.Select(s => new Member { Name = s.Name, UserId = s.Id, IsAdministrator = s.IsAdministrator }).ToList(),
                Currencies = new List<Currency>
                {
                    new Currency
                    {
                        Id = group.PrimaryCurrency.Id,
                        Name = group.PrimaryCurrency.Name,
                        Symbol = group.PrimaryCurrency.Symbol,
                        Rate = 1,
                        IsPrimary = true
                    }
                },
                Purchases = purchases.Select(s => new Purchase
                {
                    Name = s.Description,
                    Value = s.Amount,
                    PurchasedBy = s.PurchaserName,
                    Id = s.Id,
                    Date = s.Date,
                    Currency = new Currency { Name = s.Currency.Name, Symbol = s.Currency.Symbol }
                }).OrderByDescending(s => s.Date).ToList()
            };

            currencies?.ForEach(s => model.Currencies.Add(new Currency { Id = s.Id, Name = s.Name, Symbol = s.Symbol, Rate = s.Rate}));

            return View(model);
        }

        public IActionResult Create()
        {
            var currencies = _lookupManager.GetAllCurrencies();

            return View(new CreateViewModel()
            {
                Currencies = currencies.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = $"{s.Name} - {s.Symbol}", Selected = s.Symbol == "£" })
            });
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel model)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var primaryCurrencyId = model.PrimaryCurrency;

            var group = new GroupAdd()
            {
                Name = model.Name,
                CreatorId = user.Id,
                PrimaryCurrencyId = primaryCurrencyId
            };

            var id = _groupManager.Add(group);
            return RedirectToAction("Index", new { id = id });
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
            _groupManager.AddMembers(id, model.EmailAddresses.Split(",").ToList());
            return RedirectToAction("Index", new { id = id });
        }

        [HttpGet]
        public IActionResult AddCurrencies(int id)
        {
            var currencies = _lookupManager.GetAllCurrencies();
            var selectedCurrencies = _groupManager.GetCurencies(id);
            var checkboxList = new List<CurrencyCheckboxListItem>();

            foreach (var item in currencies)
            {
                var selectedCurrency = selectedCurrencies.SingleOrDefault(s => s.Id == item.Id);
                checkboxList.Add(new CurrencyCheckboxListItem()
                {
                    Checked = selectedCurrency != null,
                    Rate = selectedCurrency?.Rate ?? 0,
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
            _groupManager.UpdateCurrencies(id, model.CurrencyCheckboxList.Where(s => s.Checked).Select(s =>
                 new GroupCurrency()
                 {
                     Id = s.Currency.Id,
                     Rate = s.Rate
                 }));

            return RedirectToAction("Index", new { id = id });
        }

        [HttpGet]
        public IActionResult AddPurchase(int id)
        {
            var members = _groupManager.GetMembers(id);
            var currencies = _groupManager.GetCurencies(id);
            var model = new AddPurchaseViewModel()
            {
                Beneficiaries = new CheckboxList
                {
                    Items = members.Select(s => new CheckboxListItem { Value = s.Id, Text = s.Name }).ToList()
                },
                Currencies = currencies.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }),
                Members = members.Select(s => new SelectListItem { Value = s.Id, Text = s.Name })
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddPurchase(int id, AddPurchaseViewModel model)
        {
            var purchase = new Logic.Models.Purchase.PurchaseAddUpdate
            {
                Amount = model.Value,
                Description = model.Name,
                PurchaserId = model.Purchaser,
                CurrencyId = model.Currency,
                GroupId = id,
                Beneficiaries = model.Beneficiaries.Items.Where(s => s.IsChecked).Select(s => s.Value).ToList()
            };

            _groupManager.AddPurchase(purchase);

            return RedirectToAction("Index", new { id = id });
        }


        [HttpGet]
        public IActionResult EditPurchase(int id, int purchaseId)
        {
            var model = new { };

            return View(model);
        }
    }
}