using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Domain.Entities;
using CarTracking.BE.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Application.Vehicles.Commands;

public class AddNewVehicleCommand : IRequest<string>
{
    public required string Name { get; set; }

    public string AccountId { get; set; } = string.Empty;
}

public class AddNewVehicleCommandHandler(CarTrackingDbContext context) : IRequestHandler<AddNewVehicleCommand, string>
{
    public async Task<string> Handle(AddNewVehicleCommand request, CancellationToken cancellationToken)
    {
        var account = await
            context.Accounts.Include(x => x.Vehicles).FirstOrDefaultAsync(x => x.Id == request.AccountId,
                cancellationToken: cancellationToken);
        account.ValidateIfNotNull(request.AccountId);
        
        var vehicle = new Vehicle()
        {
            Name = request.Name,
            IsAssignedToDevice = false,
            Settings = new VehicleSettings(),
            StatusHistory = new List<Domain.Entities.Status>()
        };

        account!.Vehicles.Add(vehicle);
        await context.SaveChangesAsync(cancellationToken);
        return account.Id;
    }
}