using Sds.ReceiptShare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sds.ReceiptShare.Data
{
    public class DataInitialiser
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            //if (context.Parties.Any())
            //{
            //    return;   // DB has been seeded
            //}

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

            var primaryCurrency = new Currency() { Symbol = "£", Name = "Pound" };
            var purchaseCurrency = new Currency() { Symbol = "€", Name = "Euro" };

            context.Currencies.Add(primaryCurrency);
            context.SaveChanges();

            var group = new Group()
            {
                Name = "Snow Ballers",
                PrimaryCurrency = primaryCurrency,
                Created = DateTime.Now,
                Administrator = member1,
                GroupCurrencies = new List<GroupCurrency> { new GroupCurrency { Currency = purchaseCurrency, ConvertionRate = 1.3 } }
            };

            context.Groups.Add(group);
            context.SaveChanges();

            var groupMembers = members.Select(s => new GroupMember() { GroupId = group.Id, MemberId = s.Id });
            context.GroupMembers.AddRange(groupMembers);

            context.SaveChanges();
        }
    }
}
