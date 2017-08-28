using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sds.ReceiptShare.Domain.Entities;

namespace Sds.ReceiptShare.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
     //   public DbSet<Member> Members { get; set; }
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
            //modelBuilder.Entity<ApplicationUser>().HasOne(u => u.Member).WithOne(m => m.ApplicationUser).HasForeignKey<Member>(u => u.ApplicationUserId);

            modelBuilder.Entity<ApplicationUser>().HasMany(m => m.Groups).WithOne(g => g.Member).HasForeignKey(m => m.MemberId);
            modelBuilder.Entity<GroupMember>().HasKey(gm => new { gm.MemberId, gm.GroupId });
            
            modelBuilder.Entity<Currency>();
            modelBuilder.Entity<GroupCurrency>().HasKey(gc => new { gc.CurrencyId, gc.GroupId });

            base.OnModelCreating(modelBuilder);

            //            modelBuilder.Entity<Purchase>().ToTable("Purchase");
        }
    }

}
