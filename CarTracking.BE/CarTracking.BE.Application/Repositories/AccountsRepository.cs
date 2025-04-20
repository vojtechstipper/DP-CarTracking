using CarTracking.BE.Domain.Entities;
using CarTracking.BE.Infrastructure;
using CarTracking.BE.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Application.Repositories;

public interface IAccountsRepository : IRepository<Account>
{
    Task<Account?> GetAccountForUser(string userId);
    Task<Account?> GetByCode(string code);
}

public class AccountsRepository(CarTrackingDbContext context) : Repository<Account>(context), IAccountsRepository
{
    public async Task<Account?> GetAccountForUser(string userId)
    {
        return await context.Accounts
            .Where(x => x.UserIds.Contains(userId))
            .FirstOrDefaultAsync();
    }

    public async Task<Account?> GetByCode(string code)
    {
        return await context.Accounts.FirstOrDefaultAsync(x => x.Code == code);
    }
}