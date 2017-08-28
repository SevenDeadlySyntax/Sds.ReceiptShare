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

        public GroupController(IGroupManager groupManager, ApplicationUserManager userManager)
        {
            _groupManager = groupManager;
            _userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            var group = _groupManager.Get(id);
            var members = _groupManager.GetMembers(id);

            var model = new DetailsViewModel()
            {
                Id = group.Id,
                Name = group.Name,
                CreatedOn = group.Created,
                Members = members?.Select(s => new GroupMember { Name = s.Member.Name, UserId = s.Member.Id, IsAdministrator = s.IsAdministrator }).ToList()
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
    }
}