using CarTracking.BE.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Application.Vehicles.Commands.Trips;

public class GenerateTripsFromStatusesCommand : IRequest<int>
{
    public string VehicleId { get; set; }
}

public class GenerateTripsFromStatusesCommandHandler(IStatusRepository statusRepository) : IRequestHandler<GenerateTripsFromStatusesCommand, int>
{
    public async Task<int> Handle(GenerateTripsFromStatusesCommand request, CancellationToken cancellationToken)
    {
        var statuses =
            (await statusRepository.GetAllStatusesForVehicle(request.VehicleId).ToListAsync())
            .OrderBy(x => x.Received);

        List<List<Domain.Entities.Status>> trips = new List<List<Domain.Entities.Status>>();
        List<Domain.Entities.Status> currentTrip = new List<Domain.Entities.Status>();
        TimeSpan timeThreshold = TimeSpan.FromMinutes(10); // Set your desired time threshold

        foreach (var status in statuses)
        {
            // Determine if the vehicle is moving
            bool isMoving = (status.Received - status.StartTime).TotalMinutes <= 5;

            if (isMoving)
            {
                // If this is the first status in the current trip or if the time difference is within the threshold
                if (currentTrip.Count == 0 || (status.Received - currentTrip.Last().Received) <= timeThreshold)
                {
                    currentTrip.Add(status); // Add to the current trip
                }
                else
                {
                    // Start a new trip if the time difference is too large
                    trips.Add(new List<Domain.Entities.Status>(currentTrip));
                    currentTrip.Clear();
                    currentTrip.Add(status);
                }
            }
            else if (currentTrip.Count > 0)
            {
                // End the current trip when the vehicle stops moving
                trips.Add(new List<Domain.Entities.Status>(currentTrip));
                currentTrip.Clear();
            }
        }

// Add the last trip if it has statuses
        if (currentTrip.Count > 0)
        {
            trips.Add(currentTrip);
        }

        var reallyMoving = trips.Where(x => x.Count > 5).ToList();

        foreach (var item in reallyMoving)
        {
            var tripId = Guid.NewGuid().ToString();
            item.ForEach(x=>x.TripId = tripId);
            item.ForEach(x=>statusRepository.Update(x));
        }

        await statusRepository.SaveAsync();
        return reallyMoving.Count;
    }
}