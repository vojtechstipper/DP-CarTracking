using System.Text;
using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Application.Services;
using CarTracking.BE.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarTracking.BE.Application.Users.Commands;

public class ResetPasswordCommand : IRequest<string>
{
    public string Email { get; set; }
}

public class ResetPasswordCommandHandler(
    UserManager<IdentityUser> userManager,
    IPasswordResetCodesRepository passwordResetCodesRepository,
    IAuthentificaitonService authService,
    IEmailSender emailSender) : IRequestHandler<ResetPasswordCommand, string>
{
    public async Task<string> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null) return "";

        string code = GeneratedCode();

        PasswordResetCode passwordResetCode = new()
        {
            Code = code,
            ValidTill = DateTime.Now.AddMinutes(30),
            UserId = user.Id,
            Token = await authService.GeneratePasswordResetToken(user)
        };

        passwordResetCodesRepository.Add(passwordResetCode);
        await passwordResetCodesRepository.SaveAsync();
        
        await emailSender.SendPassworResetEmail(user!.Email, code);

        return "";
    }

    private string GeneratedCode()
    {
        Random random = new Random();

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < 6; i++)
        {
            stringBuilder.Append(chars[random.Next(chars.Length)]);
        }

        return stringBuilder.ToString();
    }
}