using System.Security.Cryptography;
using System.Text;
using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarTracking.BE.Application.Accounts.Commands.OpenAccount;

public class OpenAccountCommand : IRequest<OpenAccountDto>
{
    public string AccountId { get; set; }
}

public class OpenAccountCommandHandler(IHttpContextAccessor httpContextAccessor, IAccountsRepository accountsRepository)
    : IRequestHandler<OpenAccountCommand, OpenAccountDto>
{
    public async Task<OpenAccountDto> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
    {
        var accountId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "AccountId").Value;

        var account = await accountsRepository.GetById(accountId);

        account.ValidateIfNotNull(accountId);

        account!.Code = GenerateUniqueCode(8);
        account.CodeValidTill = DateTime.Now.AddMinutes(10);
        accountsRepository.Update(account);
        await accountsRepository.SaveAsync();

        return new(){Code = account.Code};
    }

    public string GenerateUniqueCode(int codeLength)
    {
        string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        var code = new StringBuilder(codeLength);
        var randomBytes = new byte[sizeof(uint)];

        using (var rng = RandomNumberGenerator.Create())
        {
            for (int i = 0; i < codeLength; i++)
            {
                rng.GetBytes(randomBytes);
                uint randomNumber = BitConverter.ToUInt32(randomBytes, 0);
                code.Append(characters[(int)(randomNumber % (uint)characters.Length)]);
            }
        }

        return code.ToString();
    }
}