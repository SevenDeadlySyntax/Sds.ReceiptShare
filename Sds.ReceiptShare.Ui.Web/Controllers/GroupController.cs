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


        /// <summary>
        /// TODO: I think this ought to be in the manager classes (along with the isAdmin check) so that any calls to those 
        /// methods are secured correctly. Perhaps they could throw a custom exception back to here which will result in the controller 
        /// method returning an UnauthorizedResult.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private bool IsInGroup(int groupId)
        {
            var userId = HttpContext.User.Claims.First(s => s.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            return _userManager.IsInGroup(userId, groupId);
        }

        private bool IsAdministrator(int groupId)
        {
            var userId = HttpContext.User.Claims.First(s => s.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            return _userManager.IsAdministrator(userId, groupId);
        }

        public IActionResult Index(int id)
        {
            // If no id is provided, then FUCK THIS. You're going home.
            if (id == 0) return Redirect("/");

            // You better be in the group too. TODO: I've tried to implement this as an Auth module but it was just too flaky. What else can we do?
            if (!IsInGroup(id)) return new UnauthorizedResult();
 
            var group = _groupManager.GetDetails(id);
            if (group == null) return new NotFoundResult();

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
                    Currency = new Currency { Name = s.Currency.Name, Symbol = s.Currency.Symbol },
                    Beneficiaries = new PurchaseBeneficiaries(s.Beneficiaries, s.Amount)
                }).OrderByDescending(s => s.Date).ToList()
            };

            currencies?.ForEach(s => model.Currencies.Add(new Currency { Id = s.Id, Name = s.Name, Symbol = s.Symbol, Rate = s.Rate }));

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
            if (!IsAdministrator(id)) return new UnauthorizedResult();

            var members = _groupManager.GetMembers(id);
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            return View(new AddMembersViewModel()
            {
                GroupId = id,
                EmailAddresses = string.Join(",", members.Where(s=>s.Email != user.Email).Select(s => s.Email).ToArray())
            });
        }

        [HttpPost()]
        public IActionResult AddMembers(int id, AddMembersViewModel model)
        {
            if (!IsAdministrator(id)) return new UnauthorizedResult();
            _groupManager.ManageMembers(id, model.EmailAddresses.Split(",").Where(s => !string.IsNullOrEmpty(s)).ToList());
            return RedirectToAction("Index", new { id = id });
        }

        [HttpGet]
        public IActionResult AddCurrencies(int id)
        {
            if (!IsAdministrator(id)) return new UnauthorizedResult();

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
            if (!IsAdministrator(id)) return new UnauthorizedResult();

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
            if (!IsInGroup(id)) return new UnauthorizedResult();

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
            if (!IsInGroup(id)) return new UnauthorizedResult();

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
            if (!IsInGroup(id)) return new UnauthorizedResult();

            var model = new { };
            return View(model);
        }
    }
}