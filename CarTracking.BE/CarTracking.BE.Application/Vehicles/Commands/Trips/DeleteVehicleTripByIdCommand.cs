using CarTracking.BE.Application.Repositories;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Commands.Trips;

public class DeleteVehicleTripByIdCommand : IRequest<int>
{
    public string VehicleId { get; set; }
    public string TripId { get; set; }
}

public class DeleteVehicleTripByIdCommandHandler(IStatusRepository statusRepository)
    : IRequestHandler<DeleteVehicleTripByIdCommand, int>
{
    public async Task<int> Handle(DeleteVehicleTripByIdCommand request, CancellationToken cancellationToken)
    {
        var statusesToDelete = await statusRepository.GetStatusesForTrip(request.VehicleId, request.TripId);

        statusesToDelete.ForEach(x => statusRepository.Delete(x));

        await statusRepository.SaveAsync();
        return statusesToDelete.Count;
    }
}