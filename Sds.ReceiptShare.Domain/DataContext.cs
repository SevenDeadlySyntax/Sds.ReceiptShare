using Microsoft.EntityFrameworkCore;
using Sds.ReceiptShare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sds.ReceiptShare.Domain
{
    public class DataContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<GroupCurrency> GroupCurrencies { get; set; }
        public DbSet<Purchase> Purchases{ get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(new DbContextOptions<DataContext>())
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().ToTable("Member");
            modelBuilder.Entity<Currency>().ToTable("Currency");
            modelBuilder.Entity<Group>().ToTable("Group");
            modelBuilder.Entity<GroupCurrency>().ToTable("GroupCurrency");
            modelBuilder.Entity<Purchase>().ToTable("Purchase");
        }
    }

}
