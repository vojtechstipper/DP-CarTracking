using CarTracking.BE.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CarTracking.BE.Application.Statuses.Commands;

public record CacheCheckCommand : IRequest<Unit>;

public class CacheCheckCommandHandler(IVehiclesRepository vehiclesRepository, IMemoryCache memoryCache)
    : IRequestHandler<CacheCheckCommand, Unit>
{
    public async Task<Unit> Handle(CacheCheckCommand request, CancellationToken cancellationToken)
    {
        var vehicles = (await vehiclesRepository.GetAllVehicles()).Where(x=>!string.IsNullOrEmpty(x.DeviceId) ).Select(x => x.DeviceId).ToList();

        foreach (var key in vehicles)
        {
            Domain.Entities.Status? status;
            if (!memoryCache.TryGetValue(key, out status))
            {
                Console.WriteLine($"Key {key} not found in cache");
            }
        }

        return Unit.Value;
    }
}