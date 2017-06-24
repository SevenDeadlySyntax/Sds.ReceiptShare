using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sds.ReceiptShare.Api.Models;
using Sds.ReceiptShare.Domain;
using Microsoft.EntityFrameworkCore;

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
                .Include(s => s.PurchaseCurrencies).ThenInclude(s => s.Currency)
                .FirstOrDefault(s => s.Id == id);

            if (group != null)
            {

                var response = new GroupDetails()
                {
                    Id = group.Id,
                    Name = group.Name,
                    Currencies = group.PurchaseCurrencies.Select(x => new Currency()
                    {
                        Id = x.Currency.Id,
                        Name = x.Currency.Name,
                        Symbol = x.Currency.Symbol,
                        Rate = x.ConvertionRate
                    }),
                    PrimaryCurrency = new Currency()
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
        /// Adds a new group
        /// TODO - need to identify the user and set them as the owner
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public IActionResult Post([FromBody]Group group)
        {
            var administrator = _context.Members.First(s => s.Id == group.Id);
            var newGroup = _context.Add(new Domain.Models.Group { Name = group.Name, Created = DateTime.Now, Administrator = administrator });
            _context.SaveChanges();

            return Ok(newGroup)
        }

        // PUT api/values/5
        [HttpPut("{id}/AddMembers")]
        public IActionResult Members(int id, [FromBody]int[] ids)
        {
            var group = _context.Groups.FirstOrDefault(s => s.Id == id);

            if (group == null) return NoContent();
            var successCount = 0;

            foreach (var memberId in ids)
            {
                var member = _context.Members.FirstOrDefault(s => s.Id == memberId);
                if (member != null)
                {
                    group.Members.Add(new Domain.Models.GroupMember { Member = member });
                    successCount++;
                }
            }
            _context.SaveChanges();

            return Ok($"{successCount} members added to group {id}");
        }
    }
}