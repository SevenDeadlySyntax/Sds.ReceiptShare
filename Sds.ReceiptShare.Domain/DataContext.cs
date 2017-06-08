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
        public DbSet<Party> Parties { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<PartyCurrency> PartyCurrencies { get; set; }
        public DbSet<Purchase> Purchases{ get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().ToTable("Member");
            modelBuilder.Entity<Currency>().ToTable("Currency");
            modelBuilder.Entity<Party>().ToTable("Party");
            modelBuilder.Entity<PartyCurrency>().ToTable("PartyCurrency");
            modelBuilder.Entity<Purchase>().ToTable("Purchase");
        }
    }

}
