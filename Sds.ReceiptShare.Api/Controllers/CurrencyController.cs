using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sds.ReceiptShare.Api.Models;
using Sds.ReceiptShare.Domain;

namespace Sds.ReceiptShare.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Currecny")]
    public class CurrencyController : Controller
    {
        private readonly DataContext _context;

        public CurrencyController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // Get The groups from the database
            var currencies = _context.Currencies;

            if (currencies == null || !currencies.Any()) return NoContent();

            return Ok(currencies.Select(s => new Currency()
            {
                Id = s.Id,
                Name = s.Name,
                Symbol = s.Symbol
            }));
        }

        /// <summary>
        /// Adds a new member
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public IActionResult Post([FromBody]string name, string symbol)
        {
            var newCurrecny = _context.Add(new Domain.Models.Currency()
            {
                Name = name,
                Symbol = symbol
            });

            _context.SaveChanges();

            return Ok(newCurrecny);
        }
    }
}