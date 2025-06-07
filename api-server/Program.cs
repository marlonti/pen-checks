using System.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddDbContext<BankDbContext>(o => o.UseInMemoryDatabase("TestDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

app.UseHttpsRedirection();

app.MapGet("/accounts", (IMediator mediator) =>
{
    return mediator.Send(new GetAccountsQuery());
});

app.MapGet("/transactions", (IMediator mediator) =>
{
    return mediator.Send(new GetTransactionsQuery());
});

app.MapPost("/transactions", async (PostTransactionRequest request, IMediator mediator) =>
{
    if (request.Amount == 0)
        return Results.BadRequest("What am I to do with zero?");
    try
    {
        if (request.Amount < 0)
            await mediator.Send(new PostWithdrawalCommand(request.AccountId, request.Amount));
        else
            await mediator.Send(new PostDepositCommand(request.AccountId, request.Amount));
    }
    catch (CustomValidationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    return Results.Ok();
});

app.MapPost("/transfers", async (PostTransferRequest request, IMediator mediator) =>
{
    if (request.Amount == 0)
        return Results.BadRequest("What am I to do with zero?");
    try
    {
        using var scope = new TransactionScope();
        await mediator.Send(new PostWithdrawalCommand(request.FromAccountId, request.Amount * -1));
        await mediator.Send(new PostDepositCommand(request.ToAccountId, request.Amount));
        scope.Complete();
    }
    catch (CustomValidationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    return Results.Ok();
});

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<BankDbContext>();
dbContext.Database.EnsureCreated();
app.Run();


record PostTransactionRequest(int AccountId, decimal Amount);
record PostTransferRequest(int FromAccountId, int ToAccountId, decimal Amount);

