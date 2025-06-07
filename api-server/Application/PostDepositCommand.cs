using MediatR;

public class PostDepositCommand : IRequest
{
    public PostDepositCommand(int accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
    }

    public int AccountId { get; }
    public decimal Amount { get; }
}

public class PostDepositCommandHandler : IRequestHandler<PostDepositCommand>
{
    private readonly BankDbContext _dbContext;

    public PostDepositCommandHandler(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(PostDepositCommand request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts.FindAsync(request.AccountId) ??
            throw new CustomValidationException("Invalid Account");

        var transacton = new AccountTransaction()
        {
            AccountId = request.AccountId,
            Amount = request.Amount
        };
        _dbContext.Transactions.Add(transacton);
        await _dbContext.SaveChangesAsync();

    }
}