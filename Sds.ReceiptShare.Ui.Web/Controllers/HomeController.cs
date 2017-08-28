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
using Sds.ReceiptShare.Logic.Managers;

namespace Sds.ReceiptShare.Ui.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IGroupManager _groupManager;
        //private readonly IMemberManager _memberManager;
        private readonly ApplicationUserManager _userManager;

        public HomeController(IGroupManager groupManager, ApplicationUserManager userManager)
        {
            _groupManager = groupManager;
            _userManager = userManager;

        }

        // An aothorized user will be shown their list of groups
        // If they have not yet set a user name, the are redirected to do so
        public async Task<IActionResult> Index()
        {
            // Get the authenticated user
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            if (string.IsNullOrEmpty(user.UserName))
            {
                return RedirectToAction("SetName");
            }

            // Fetch the athenticated user's groups
            IEnumerable<GroupMember> groups = _userManager.GetGroups(user);

            var homeViewModel = new HomeViewModel()
            {
                UserName = user.Name,
                Groups = groups?.Select(s => new GroupListItem() { Name = s.Member.Name, NumberOfMembers = s.Group.Members.Count }).ToList()
            };

            return View(homeViewModel);
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
