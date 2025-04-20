using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarTracking.BE.Application.Users.Commands;

public record LoginUserCommand(string Username, string Password) : IRequest<LoginDto>;

public class LoginUserCommandHandler(
    IAccountsRepository accountsRepository,
    IAuthentificaitonService authService,
    UserManager<IdentityUser> userManager)
    : IRequestHandler<LoginUserCommand, LoginDto>
{
    public async Task<LoginDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Username);
        if (user is null) return new LoginDto() { Username = "", Token = "" };

        var token = await authService.ValidateAndGenerateToken(request.Username, request.Password);

        if (string.IsNullOrEmpty(token))
            return new LoginDto() { Username = "", Token = "" };

        var account = await accountsRepository.GetAccountForUser(user.Id);

        return new LoginDto() { Username = request.Username, Token = token, IsAssignedToAccount = account != null };
    }
}