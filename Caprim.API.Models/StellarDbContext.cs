using Microsoft.EntityFrameworkCore;

namespace Caprim.API.Models
{
    /// <summary>
    /// DbContext for Stellar database
    /// </summary>
    public class StellarDbContext : DbContext
    {
        public StellarDbContext(DbContextOptions<StellarDbContext> options) : base(options) { }

        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<KycLevel> KycLevels { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccountType> BankAccountTypes { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<StellarAccount> StellarAccounts { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<ExchangeRateHistory> ExchangeRatesHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserStatus)
                .WithMany(us => us.Users)
                .HasForeignKey(u => u.UserStatusId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.KycLevel)
                .WithMany(kl => kl.Users)
                .HasForeignKey(u => u.KycLevelId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserProfile)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.UserId);

            modelBuilder.Entity<UserProfile>()
                .HasOne<DocumentType>()
                .WithMany()
                .HasForeignKey(up => up.DocumentTypeId);

            // StellarAccount belongs to User
            modelBuilder.Entity<StellarAccount>()
                .HasOne<User>()
                .WithMany(u => u.StellarAccounts)
                .HasForeignKey(sa => sa.UserId);

            modelBuilder.Entity<BankAccount>()
                .HasOne<User>()
                .WithMany(u => u.BankAccounts)
                .HasForeignKey(ba => ba.UserId);

            modelBuilder.Entity<BankAccount>()
                .HasOne<Bank>()
                .WithMany()
                .HasForeignKey(ba => ba.BankId);

            modelBuilder.Entity<BankAccount>()
                .HasOne<BankAccountType>()
                .WithMany()
                .HasForeignKey(ba => ba.AccountTypeId);

            modelBuilder.Entity<BankAccount>()
                .HasOne<DocumentType>()
                .WithMany()
                .HasForeignKey(ba => ba.HolderDocumentTypeId);

            modelBuilder.Entity<Transaction>()
                .HasOne<StellarAccount>()
                .WithMany()
                .HasForeignKey(t => t.StellarAccountId);

            modelBuilder.Entity<Transaction>()
                .HasOne<Asset>()
                .WithMany()
                .HasForeignKey(t => t.AssetId);

            modelBuilder.Entity<ExchangeRate>()
                .HasOne<Asset>()
                .WithMany()
                .HasForeignKey(er => er.BaseAssetId)
                .OnDelete(DeleteBehavior.Restrict); // To avoid multiple cascade paths

            modelBuilder.Entity<ExchangeRate>()
                .HasOne<Asset>()
                .WithMany()
                .HasForeignKey(er => er.QuoteAssetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExchangeRateHistory>()
                .HasOne<ExchangeRate>()
                .WithMany()
                .HasForeignKey(erh => erh.ExchangeRateId);
        }
    }
}