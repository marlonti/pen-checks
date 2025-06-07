using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetTransactionsQuery : IRequest<IEnumerable<TransactionModel>>
{
}

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, IEnumerable<TransactionModel>>
{
    private readonly BankDbContext _dbContext;

    public GetTransactionsQueryHandler(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TransactionModel>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.Transactions.Select(x => new TransactionModel()
        {
            Id = x.Id,
            AccountName = x.Account!.Name,
            Amount = x.Amount
        }).ToListAsync();
        return list;
    }
}

public class TransactionModel
{
    public int Id { get; set; }
    public required string AccountName { get; set; }
    public decimal Amount { get; set; }
}