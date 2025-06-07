using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAccountsQuery : IRequest<IEnumerable<BankAccount>>
{
}

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, IEnumerable<BankAccount>>
{
    private readonly BankDbContext _dbContext;

    public GetAccountsQueryHandler(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<BankAccount>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Accounts
            .Select(a => new BankAccount(a.Id, a.Name, a.Transactions.Sum(t => t.Amount)))
            .ToListAsync();
    }
}

public record BankAccount(int Id, string Name, decimal Balance);