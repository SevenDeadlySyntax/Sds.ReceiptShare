using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sds.ReceiptShare.Api.Models;
using Sds.ReceiptShare.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Sds.ReceiptShare.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Groups")]
    public class GroupsController : Controller
    {
        private readonly DataContext _context;

        public GroupsController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns details for a given group id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var group = _context.Groups
                .Include(s => s.Members)
                .Include(s => s.Administrator)
                .Include(s => s.PrimaryCurrency)
                .Include(s => s.GroupCurrencies).ThenInclude(s => s.Currency)
                .FirstOrDefault(s => s.Id == id);

            if (group != null)
            {

                var response = new GroupDetails()
                {
                    Id = group.Id,
                    Name = group.Name,
                    Currencies = group.GroupCurrencies.Select(x => new Currency()
                    {
                        Id = x.Currency.Id,
                        Name = x.Currency.Name,
                        Symbol = x.Currency.Symbol,
                        Rate = x.ConvertionRate
                    }),
                    PrimaryCurrency = group.PrimaryCurrency == null ? null : new Currency()
                    {
                        Name = group.PrimaryCurrency.Name,
                        Symbol = group.PrimaryCurrency.Symbol,
                        Id = group.PrimaryCurrency.Id
                    },
                    AdminName = group.Administrator.Name
                };
                return Ok(response);
            }

            return NotFound();
        }

        /// <summary>
        /// Gets the members of a specified group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/Members")]
        public IActionResult GetMembers(int id)
        {

            var group = _context.Groups.Include(s=> s.Administrator).FirstOrDefault(s => s.Id == id);
            if (group == null) return NotFound();

            var groupMembers = _context.GroupMembers.Include(s => s.Member)
                .Where(s => s.GroupId == id)
                .Select(s => new GroupMember { Id = s.MemberId, Name = s.Member.Name }).ToList();
            groupMembers.Add(new GroupMember { Id = group.Administrator.Id, Name = group.Administrator.Name, IsAdmin = true });

            if (!groupMembers.Any()) return NoContent();

            return Ok(groupMembers);
        }

        /// <summary>
        /// Adds a new group
        /// TODO - need to identify the user and set them as the owner
        /// Example request body: {"Id": 0,"Name": "Test Group","AdminId": 1}
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public IActionResult Post([FromBody]Group group)
        {
            if (group == null || group.Name == null || group.AdminId == 0) return BadRequest("Required fields not provided.");
            var administrator = _context.Members.First(s => s.Id == group.AdminId);
            var newGroup = _context.Add(new Domain.Models.Group { Name = group.Name, Created = DateTime.Now, Administrator = administrator });
            _context.SaveChanges();

            return Ok(newGroup);
        }

        /// <summary>
        /// Sample body: [2,3]
        /// Sample URL: api/groups/1/Members
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut("{id}/Members")]
        public IActionResult Members(int id, [FromBody]int[] ids)
        {
            var successCount = 0;

            // If no member ids, return bad request
            if (ids == null || !ids.Any()) return BadRequest("No members specified");

            // If group not found, return Not Found
            var group = _context.Groups
                .Include(s => s.Members)
                .Include(s=> s.Administrator)
                .FirstOrDefault(s => s.Id == id);
            if (group == null) return NotFound();

            if (group.Members == null) group.Members = new List<Domain.Models.GroupMember>();

            // For each member specified...
            foreach (var memberId in ids)
            {
                // ...Check that the member does not already exist in the group (and is not the admin)...
                if (group.Members.Any(s => s.MemberId == memberId) || group.Administrator.Id == memberId) continue;

                // ...Get the matching member record...
                var member = _context.Members.FirstOrDefault(s => s.Id == memberId);
                if (member != null)
                {
                    // ...Create new group member record...
                    // _context.GroupMembers.Add(new Domain.Models.GroupMember { MemberId = memberId });
                    group.Members.Add(new Domain.Models.GroupMember { MemberId = memberId });
                    successCount++;
                }

            }
            _context.SaveChanges();

            return Ok($"{successCount} members added to group {id}");
        }

        /// <summary>
        /// Add purchase currencies to a group
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}/Currencies")]
        public IActionResult Currencies(int id, [FromBody]int[] ids)
        {
            var successCount = 0;

            // If no member ids, return bad request
            if (ids == null || !ids.Any()) return BadRequest("No currencies specified");

            // Get the group
            var group = _context.Groups
                .Include(s => s.GroupCurrencies)
                .Include(s=>s.PrimaryCurrency)
                .FirstOrDefault(s => s.Id == id);


            // If group not found, return Not Found
            if (group == null) return NotFound();

            //
            if (group.GroupCurrencies == null) group.GroupCurrencies = new List<Domain.Models.GroupCurrency>();

            // For each member specified...
            foreach (var currencyId in ids)
            {
                // ...Check that the member does not already exist in the group (and is not the admin)...
                if (group.GroupCurrencies.Any(s => s.CurrencyId == currencyId) || group.PrimaryCurrency.Id == currencyId) continue;

                // ...Get the matching member record...
                var currency = _context.Currencies.FirstOrDefault(s => s.Id == currencyId);
                if (currency != null)
                {
                    // ...Create new group member record...
                    group.GroupCurrencies.Add(new Domain.Models.GroupCurrency { CurrencyId = currencyId });
                    successCount++;
                }

            }
            _context.SaveChanges();

            return Ok($"{successCount} currencies added to group {id}");
        }
    }
}