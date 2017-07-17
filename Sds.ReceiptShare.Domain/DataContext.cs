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
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<GroupCurrency> GroupCurrencies { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Group>().HasMany(g => g.Members).WithOne(m => m.Group).HasForeignKey(g => g.GroupId);
            modelBuilder.Entity<Group>().HasMany(g => g.GroupCurrencies).WithOne(m => m.Group).HasForeignKey(g => g.GroupId);

            modelBuilder.Entity<Member>().HasMany(m => m.Groups).WithOne(g => g.Member).HasForeignKey(m => m.MemberId);
            modelBuilder.Entity<GroupMember>().HasKey(gm => new { gm.MemberId, gm.GroupId });
            
            modelBuilder.Entity<Currency>();
            modelBuilder.Entity<GroupCurrency>().HasKey(gc => new { gc.CurrencyId, gc.GroupId });
            
            
//            modelBuilder.Entity<Purchase>().ToTable("Purchase");
        }
    }

}
