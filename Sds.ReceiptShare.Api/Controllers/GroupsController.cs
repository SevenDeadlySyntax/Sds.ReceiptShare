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
                .Include(s=> s.Members)
                .Include(s=> s.Administrator)
                .Include(s=> s.PrimaryCurrency)
                .Include(s=> s.PurchaseCurrencies).ThenInclude(s=> s.Currency)
                .FirstOrDefault(s => s.Id == id);

            if (group != null) {

                var response = new UserGroup()
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
        public void Post([FromBody]string name)
        {
            var party = new Domain.Models.Group()
            {
                Name = name
            };

            _context.Add(party);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}