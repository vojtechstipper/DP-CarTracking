using CarTracking.BE.Application.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CarTracking.BE.Application.Services;

public interface IAuthentificaitonService
{
    Task<IdentityUser?> CheckUser(string email, string password);
    Task<string?> ValidateAndGenerateToken(string email, string password);
    Task<IdentityUser?> RegisterUser(string requestEmail, string requestPassword);
    Task<string> GeneratePasswordResetToken(IdentityUser email);
    Task<bool> SetNewPassword(IdentityUser user, string token, string newPassword);
}

public class AuthentificationService(
    UserManager<IdentityUser> userManager,
    ITokenService tokenService,
    IAccountsRepository accountsRepository)
    : IAuthentificaitonService
{
    public async Task<IdentityUser?> CheckUser(string email, string password)
    {
        email = NormalizeEmail(email);
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return null;
        }

        var isPasswordOk = await userManager.CheckPasswordAsync(user, password);
        return isPasswordOk ? user : null;
    }

    public async Task<string?> ValidateAndGenerateToken(string email, string password)
    {
        var user = await CheckUser(email, password);

        if (user is null) return string.Empty;

        var account = await accountsRepository.GetAccountForUser(user.Id);
        Dictionary<string, string> data = new()
        {
            { "UserId", user.Id },
            { "AccountId", account?.Id ?? string.Empty }
        };
        return tokenService.GenerateToken(data);
    }

    public async Task<IdentityUser?> RegisterUser(string requestEmail, string requestPassword)
    {
        var userExists = await CheckUserExistence(requestEmail);
        if (userExists) return null;
        requestEmail = NormalizeEmail(requestEmail);
        IdentityUser user = new()
        {
            Email = requestEmail,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = requestEmail
        };
        var result = await userManager.CreateAsync(user, requestPassword);
        if (!result.Succeeded) return null;
        return user;
    }

    public async Task<string> GeneratePasswordResetToken(IdentityUser user)
    {
        return await userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<bool> SetNewPassword(IdentityUser user, string token, string newPassword)
    {
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded;
    }

    private async Task<bool> CheckUserExistence(string requestEmail)
    {
        var user = await userManager.FindByEmailAsync(requestEmail);
        return user != null;
    }

    private string NormalizeEmail(string email)
    {
        return email.ToLower().Normalize();
    }
}