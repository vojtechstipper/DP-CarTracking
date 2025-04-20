using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarTracking.BE.Application.Accounts.Commands.JoinAccount;

public class JoinAccountCommand : IRequest<CreateOrJoinAccountDto>
{
    public string Code { get; set; }
}

public class JoinAccountCommandHandler(
    ITokenService tokenService,
    IAccountsRepository accountsRepository,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<JoinAccountCommand, CreateOrJoinAccountDto>
{
    public async Task<CreateOrJoinAccountDto> Handle(JoinAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await accountsRepository.GetByCode(request.Code);

        //pokud account null tak relevantní excepiton
        if (account is null) throw new Exception($"Account with code {request.Code} was not found");

        if (account.CodeValidTill < DateTime.Now) throw new Exception("Code is expired");

        var userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;

        if (!string.IsNullOrEmpty(userId))
        {
            account.UserIds.Add(userId);
            account.UserIds = account.UserIds.Distinct().ToList();
            accountsRepository.Update(account);
            await accountsRepository.SaveAsync();
        }

        string token = GenerateToken(account.Id, userId);
        return new() { Token = token };
    }

    private string GenerateToken(string accountId, string userId)
    {
        Dictionary<string, string> data = new()
        {
            { "UserId", userId },
            { "AccountId", accountId }
        };
        return tokenService.GenerateToken(data);
    }
}