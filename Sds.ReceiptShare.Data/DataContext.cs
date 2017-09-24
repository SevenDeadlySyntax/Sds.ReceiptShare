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
        public DbSet<PurchaseBeneficiary> PurchaseBeneficiaries { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasMany(g => g.Members).WithOne(m => m.Group).OnDelete(DeleteBehavior.Restrict).HasForeignKey(m => m.GroupId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Group>().HasMany(g => g.GroupCurrencies).WithOne(m => m.Group).OnDelete(DeleteBehavior.Restrict).HasForeignKey(m => m.GroupId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Group>().HasMany(g => g.Purchases).WithOne(p => p.Group).OnDelete(DeleteBehavior.Restrict).HasForeignKey(p => p.GroupId).OnDelete(DeleteBehavior.Restrict);
         //   modelBuilder.Entity<Group>().HasOne(g => g.PrimaryCurrency).WithMany().HasForeignKey(g => g.PrimaryCurrencyId);

            modelBuilder.Entity<ApplicationUser>().HasMany(m => m.Groups).WithOne(g => g.Member).OnDelete(DeleteBehavior.Restrict).HasForeignKey(g => g.MemberId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ApplicationUser>().HasMany(m => m.BeneficiaryOf).WithOne(pb => pb.Member).OnDelete(DeleteBehavior.Restrict).HasForeignKey(pb => pb.MemberId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupMember>().HasKey(gm => new { gm.MemberId, gm.GroupId });

            modelBuilder.Entity<Currency>();

            modelBuilder.Entity<GroupCurrency>().HasKey(gc => new { gc.CurrencyId, gc.GroupId });

            modelBuilder.Entity<Purchase>().HasMany(p => p.Beneficiaries).WithOne(b => b.Purchase).HasForeignKey(b => b.PurchaseId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Purchase>().HasOne(p => p.Currency).WithMany().HasForeignKey(p => p.CurrencyId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Purchase>().HasOne<Group>().WithMany(g => g.Purchases).HasForeignKey(g => g.GroupId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Purchase>().HasOne<ApplicationUser>().WithMany(m => m.Purchases).HasForeignKey(p => p.PurchaserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseBeneficiary>().HasKey(pb => new { pb.PurchaseId, pb.MemberId });

            base.OnModelCreating(modelBuilder);

            //            modelBuilder.Entity<Purchase>().ToTable("Purchase");
        }
    }

}
