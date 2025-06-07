using MediatR;
using Microsoft.EntityFrameworkCore;

public class PostWithdrawalCommand : IRequest
{
    public PostWithdrawalCommand(int accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
    }

    public int AccountId { get; }
    public decimal Amount { get; }
}

public class PostWithdrawalCommandHandler : IRequestHandler<PostWithdrawalCommand>
{
    private readonly BankDbContext _dbContext;

    public PostWithdrawalCommandHandler(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(PostWithdrawalCommand request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(x => x.Id == request.AccountId) ??
            throw new CustomValidationException("Invalid Account");

        if (account.Transactions.Sum(x => x.Amount) < request.Amount * -1)
            throw new CustomValidationException("Not enough money.");

        var transacton = new AccountTransaction()
        {
            AccountId = request.AccountId,
            Amount = request.Amount
        };
        _dbContext.Transactions.Add(transacton);
        await _dbContext.SaveChangesAsync();
    }
}