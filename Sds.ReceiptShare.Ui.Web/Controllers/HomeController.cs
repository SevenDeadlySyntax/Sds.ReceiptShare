using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sds.ReceiptShare.Ui.Web.Models.Home;
using Sds.ReceiptShare.Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Sds.ReceiptShare.Domain.Entities;

namespace Sds.ReceiptShare.Ui.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IGroupManager _groupManager;
        private readonly IMemberManager _memberManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _user;

        public HomeController(IGroupManager groupManager, IMemberManager memberManager, UserManager<ApplicationUser> userManager)
        {
            _groupManager = groupManager;
            _memberManager = memberManager;
            _userManager = userManager;

        }

        // An aothorized user will be shown their list of groups
        // If they have not yet set a user name, the are redirected to do so
        public async Task<IActionResult> Index()
        {
            // Get the authenticated user
            _user = _userManager.GetUserAsync(HttpContext.User).Result;

            if (_user.Member == null || string.IsNullOrEmpty(_user.Member.Name))
            {
                return RedirectToAction("SetName");
            }

            // Fetch the athenticated user's groups
            var groups = _memberManager.GetGroups(_user.Member.Id);

            var homeViewModel = new HomeViewModel()
            {
                UserName = _user.Member.Name,
                Groups = groups.Select(s => new GroupListItem() { Name = s.Name, NumberOfMembers = s.Members.Count }).ToList()
            };

            return View(homeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> SetName()
        {
            _user = _userManager.GetUserAsync(HttpContext.User).Result;
            var model = new SetNameViewModel();
            if (_user.Member == null)
            {
                _user.Member = new Member { Name = _user.UserName };
                await _userManager.UpdateAsync(_user);
            }

            model.Name = _user.Member.Name;
            return View(model);
        }

        [HttpPut]
        public async Task<IActionResult> SetName(SetNameViewModel model)
        {
            _user = _userManager.GetUserAsync(HttpContext.User).Result;
            _user.Member.Name = model.Name;
            await _userManager.UpdateAsync(_user);
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
