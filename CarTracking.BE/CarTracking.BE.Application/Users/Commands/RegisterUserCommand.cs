using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Services;
using CarTracking.BE.Infrastructure;
using MediatR;

namespace CarTracking.BE.Application.Users.Commands;

public class RegisterUserCommand : IRequest<LoginDto>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterUserCommandHandler(CarTrackingDbContext context, IAuthentificaitonService authService)
    : IRequestHandler<RegisterUserCommand, LoginDto>
{
    public async Task<LoginDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await authService.RegisterUser(request.Email, request.Password);
        if (user is null) return new LoginDto() { };
        
        await context.SaveChangesAsync(cancellationToken);

        var token = await authService.ValidateAndGenerateToken(request.Email, request.Password);
        
        return new LoginDto
        {
            Username = String.Empty,
            Token = token,
            IsAssignedToAccount = false
        };
    }
}