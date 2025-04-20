using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Application.Services;
using CarTracking.BE.Domain.Entities;
using MediatR;

namespace CarTracking.BE.Application.Accounts.Commands;

public class CreateAccountCommand : IRequest<CreateOrJoinAccountDto>
{
    public string UserId { get; set; }
}

public class CreateAccountCommandHandler(IAccountsRepository accountsRepository, ITokenService tokenService)
    : IRequestHandler<CreateAccountCommand, CreateOrJoinAccountDto>
{
    public async Task<CreateOrJoinAccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        if (!(await IsUserAssignedToAccount(request.UserId)))
        {
            var account = new Account()
            {
                Devices = [],
                Vehicles = [],
                UserIds = new List<string> { request.UserId }
            };

            accountsRepository.Add(account);
            await accountsRepository.SaveAsync();

            Dictionary<string, string> data = new()
            {
                { "UserId", request.UserId },
                { "AccountId", account?.Id ?? string.Empty }
            };
            var token = tokenService.GenerateToken(data);
            return new CreateOrJoinAccountDto() { Token = token };
        }

        //TODO create new token with account id ale zatím stačí aby v tokenu bylo userId a podle toho vždy najdu account    
        return new CreateOrJoinAccountDto() { Token = "empty" };
    }

    private async Task<bool> IsUserAssignedToAccount(string requestUserId)
    {
        var account = await accountsRepository.GetAccountForUser(requestUserId);
        return account != null;
    }
}