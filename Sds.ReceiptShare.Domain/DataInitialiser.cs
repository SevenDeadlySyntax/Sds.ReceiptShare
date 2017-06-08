using Sds.ReceiptShare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sds.ReceiptShare.Domain
{
    public class DataInitialiser
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Parties.Any())
            {
                return;   // DB has been seeded
            }

            var member1 = new Member { Name = "Ed" };
            var members = new List<Member>() {
                new Member { Name = "Barney" },
                new Member { Name = "James" },
                new Member { Name = "Sophia" },
                new Member { Name = "Alli" },
                new Member { Name = "Dave" },
                new Member { Name = "Laura" },
                new Member { Name = "Mike" },
            };
            context.Members.Add(member1);
            context.Members.AddRange(members);
            
            var currency = new Currency() { Culture = "en-GB", Name = "Pound" };
            context.Currencies.Add(currency);

            context.SaveChanges();

            var party = new Party()
            {
                Name = "Snow Ballers",
                PrimaryCurrency = currency,
                Created = DateTime.Now,
                Administrator = member1,
                Members = members,
                PurchaseCurrencies = new List<PartyCurrency> { new PartyCurrency { Currency = currency, ConvertionRate = 1 } }
            };

            context.Parties.Add(party);
            context.SaveChanges();
        }
    }
}
