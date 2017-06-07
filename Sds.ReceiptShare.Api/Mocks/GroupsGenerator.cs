using Sds.ReceiptShare.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sds.ReceiptShare.Api.Mocks
{
    public static class ModelGenerator
    {
        public static IEnumerable<Group> GenerateGroupList()
        {
            var groups = new List<Group>
            {
                new Group(){
                    Id = 0,
                    Currencies = GenerateCurrencyList(),
                    Name = "Group1"
                },
                new Group(){
                    Id = 1,
                    Currencies = GenerateCurrencyList().Where(s=> s.Name.Equals("Pounds")),
                    Name = "Group2"
                }
            };

            return groups;
        }
        public static IEnumerable<Currency> GenerateCurrencyList()
        {
            var currencies = new List<Currency>
            {
                new Currency(){
                    Name = "Pounds",
                    Symbol = "£"
                },
                new Currency(){
                    Name = "Euros",
                    Symbol = "€"
                },
                new Currency(){
                    Name = "Vietnamese Dong",
                    Symbol = "₫"
                },
            };

            return currencies;
        }
    }
}
