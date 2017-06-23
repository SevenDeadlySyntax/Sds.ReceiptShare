using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sds.ReceiptShare.Api.Models;
using Sds.ReceiptShare.Domain;

namespace Sds.ReceiptShare.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Members")]
    public class MembersController : Controller
    {
        private readonly DataContext _context;

        public MembersController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all of a member's parties
        /// TODO: Need to identify the user and return the groups for a specific user
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/groups")]
        public IActionResult Groups(int id)
        {
            // Get The groups from the database
            var groups = _context.Groups.Where(s => s.Members.Select(x => x.MemberId).Contains(id)).ToList();

            if (groups == null || !groups.Any()) return NoContent();

            // Build up a response object:
            /* 
            // Method one, write a "for each" loop, creating an object for each matched group. Then add it to a collection and return the collection. 
            var response = new List<UserGroup>();
            foreach (var group in groups)
            {
                var userGroup = new UserGroup();
                var currencies = new List<Currency>();
                var primaryCurrency = new Currency();

                foreach (var purchaseCurrency in group.PurchaseCurrencies)
                {
                    var currency = new Currency();
                    currency.Id = purchaseCurrency.Currency.Id;
                    currency.Name = purchaseCurrency.Currency.Name;
                    currencies.Add(currency);
                }

                primaryCurrency.Id = group.PrimaryCurrency.Id;
                primaryCurrency.Name = group.PrimaryCurrency.Name;
                primaryCurrency.Symbol = group.PrimaryCurrency.Symbol;

                userGroup.Id = group.Id;
                userGroup.Name = group.Name;
                userGroup.PrimaryCurrency = new Currency()
                {
                    Id = group.PrimaryCurrency.Id,
                    Name = group.PrimaryCurrency.Name,
                    Symbol = group.PrimaryCurrency.Symbol,
                };
                userGroup.Currencies = currencies;
                userGroup.AdminName = group.Administrator.Name;

                response.Add(userGroup);
            }

            return Ok(response); 
            */

            return Ok(groups.Select(s => new UserGroup() {
                Id = s.Id,
                Name = s.Name,
                Currencies = s.PurchaseCurrencies.Select(x => new Currency() {
                    Id = x.Currency.Id,
                    Name = x.Currency.Name,
                    Symbol = x.Currency.Symbol,
                    Rate = x.ConvertionRate
                }),
                PrimaryCurrency = new Currency() {
                    Name = s.PrimaryCurrency.Name,
                    Symbol = s.PrimaryCurrency.Symbol,
                    Id = s.PrimaryCurrency.Id
                },
                AdminName = s.Administrator.Name
            }));
        }

        /// <summary>
        /// Returns member details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new member
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