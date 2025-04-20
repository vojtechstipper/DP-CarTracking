using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Application.Vehicles.Queries;

public class GetVehiclesForAccountQuery : IRequest<VehiclesList>
{
    public string AccountId { get; set; }

    public GetVehiclesForAccountQuery(string accountId)
    {
        AccountId = accountId;
    }
}

public class GetVehiclesForAccountQueryHandler(CarTrackingDbContext context)
    : IRequestHandler<GetVehiclesForAccountQuery, VehiclesList>
{
    public async Task<VehiclesList> Handle(GetVehiclesForAccountQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await context.Vehicles
            .Where(x => x.AccountId == request.AccountId)
            .Select(x => new VehicleListItemDto()
            {
                Id = x.Id,
                Name = x.Name,
                IsAssigned = x.IsAssignedToDevice,
                DeviceId = x.DeviceId
            })
            .ToListAsync(cancellationToken: cancellationToken);
        return new VehiclesList() { Vehicles = vehicles };
    }
}