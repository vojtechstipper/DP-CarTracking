using CarTracking.BE.Domain.Entities;
using CarTracking.BE.Infrastructure;
using CarTracking.BE.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Application.Repositories;

public interface IPasswordResetCodesRepository : IRepository<PasswordResetCode>
{
    Task<PasswordResetCode?> GetByUserId(string userId, string code);
}

public class PasswordResetCodesRepository(CarTrackingDbContext context)
    : Repository<PasswordResetCode>(context), IPasswordResetCodesRepository
{
    public async Task<PasswordResetCode?> GetByUserId(string userId, string code)
    {
        return await context.PasswordResetCodes.FirstOrDefaultAsync(x => x.UserId == userId && x.Code == code);
    }
}