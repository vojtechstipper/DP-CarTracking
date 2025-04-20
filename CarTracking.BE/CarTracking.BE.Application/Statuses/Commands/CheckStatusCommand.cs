using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CarTracking.BE.Application.Statuses.Commands;

public record CheckStatusCommand : IRequest<Unit>;

public class CheckStatusCommandHandler(
    CarTrackingDbContext context,
    IMemoryCache memoryCache,
    IVehiclesRepository vehiclesRepository)
    : IRequestHandler<CheckStatusCommand, Unit>
{
    public async Task<Unit> Handle(CheckStatusCommand request, CancellationToken cancellationToken)
    {
        var allVehicles = await vehiclesRepository.GetAllVehicles();

        List<Domain.Entities.Status> statusesToDelete = new();
        //load threshold for each account / vehicle
        foreach (var vehicle in allVehicles)
        {
            var timeTreshold = DateTime.Now.AddDays(-vehicle.Settings.HistoryLenghtInDays);
            var statuses = await context.Statuses
                .Where(x => x.Received < timeTreshold)
                .ToListAsync(cancellationToken: cancellationToken);
            statusesToDelete.AddRange(statuses);
        }


        context.Statuses.RemoveRange(statusesToDelete);
        await context.SaveChangesAsync();

        return Unit.Value;
    }
}