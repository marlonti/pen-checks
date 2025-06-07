using Microsoft.EntityFrameworkCore;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions<BankDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountTransaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Seed
        modelBuilder.Entity<Account>().HasData(
            new Account() { Id = 1, Name = "Checking", },
            new Account() { Id = 2, Name = "Savings", }
        );

        modelBuilder.Entity<AccountTransaction>().HasData(
            new AccountTransaction() { Id = 1, AccountId = 1, Amount = 300 },
            new AccountTransaction() { Id = 2, AccountId = 2, Amount = 100 }
        );
    }
}

public class Account
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<AccountTransaction> Transactions { get; set; } = [];
}

public class AccountTransaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public Account? Account { get; set; }
}