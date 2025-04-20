using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Application.Services;
using CarTracking.BE.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarTracking.BE.Application.Users.Commands;

public class SetNewPasswordCommand : IRequest<string>
{
    public string Email { get; set; }
    public string NewPassword { get; set; }
    public string Code { get; set; }
}

public class SetNewPasswordCommandHandler(
    IPasswordResetCodesRepository passwordResetCodesRepository,
    UserManager<IdentityUser> userManager,
    IAuthentificaitonService authentificaitonService) : IRequestHandler<SetNewPasswordCommand, string>
{
    public async Task<string> Handle(SetNewPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null) return "";

        var passwordResetCode = await passwordResetCodesRepository.GetByUserId(user.Id, request.Code);

        passwordResetCode.ValidateIfNotNull(request.Code);

        await ValidateChangeUserAttempt(passwordResetCode);

        var result = await authentificaitonService.SetNewPassword(user, passwordResetCode.Token, request.NewPassword);
        if (!result) throw new Exception("Password change failed");
        return "";
    }

    private async Task ValidateChangeUserAttempt(PasswordResetCode passwordResetCode)
    {
        passwordResetCodesRepository.Delete(passwordResetCode);
        await passwordResetCodesRepository.SaveAsync();
        if (passwordResetCode.ValidTill < DateTime.Now)
        {
            throw new Exception($"Password reset code expired");
        }
    }
}