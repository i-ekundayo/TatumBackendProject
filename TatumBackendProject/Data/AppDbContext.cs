//using System.Transactions;
using Microsoft.EntityFrameworkCore;
using TatumBackendProject.Entities;

namespace TatumBackendProject.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Account> Accounts => Set<Account>();
        //public DbSet<Transaction> Transactions => Set<Transaction>();
        //public DbSet<Transfer> Transfers => Set<Transfer>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        //public DbSet<Order> Orders => Set<Order>();
        //public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            //modelBuilder.Entity<Transaction>().HasKey();
            //modelBuilder.Entity<Transfer>().HasKey(t => t.Id);

            //Idempotency index removed
            modelBuilder.Entity<RefreshToken>().HasKey(r => r.Id);
            //modelBuilder.Entity<Biller>().HasIndex(x => x.Code).IsUnique();

        }
    }
}
