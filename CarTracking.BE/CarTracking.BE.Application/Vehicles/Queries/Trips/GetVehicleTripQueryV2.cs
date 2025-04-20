using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Helpers;
using CarTracking.BE.Application.Repositories;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Queries.Trips;

public class GetVehicleTripQueryV2 : IRequest<VehicleTripsListDto>
{
    public string VehicleId { get; set; }
}

public class GetVehicleTripQueryV2Handler(
    IStatusRepository statusRepository,
    IVehicleLocationsRepository vehicleLocationsRepository)
    : IRequestHandler<GetVehicleTripQueryV2, VehicleTripsListDto>
{
    public async Task<VehicleTripsListDto> Handle(GetVehicleTripQueryV2 request, CancellationToken cancellationToken)
    {
        var trips = await statusRepository.GetTripsForVehicle(request.VehicleId);

        var tripsGroupped = trips.GroupBy(x => x.TripId).Select(x => new VehicleTripInfoDto()
            {
                TripId = x.Key!,
                LocationStart = new LocationDto()
                {
                    Latitude = x.First().Location.Latitude,
                    Longitude = x.First().Location.Longitude
                },
                LocationFinish = new LocationDto()
                {
                    Latitude = x.Last().Location.Latitude,
                    Longitude = x.Last().Location.Longitude
                },
                Locations = x.Select(y => new LocationDto()
                {
                    Longitude = y.Location.Longitude,
                    Latitude = y.Location.Latitude
                }).ToList(),
                TripStartTime = DateTimeConverterHelper.ConvertToCentralEuropeTimeZone(x.First().Received),
                TripEndTime = DateTimeConverterHelper.ConvertToCentralEuropeTimeZone(x.Last().Received),
                TripLength = x.Last().Received - x.First().Received
            })
            .OrderByDescending(x => x.TripStartTime)
            .ToList();

        var locations = await vehicleLocationsRepository.GetVehicleLocationsByVehicleId(request.VehicleId);

        if (!locations.Any())
            return new()
            {
                VehicleId = request.VehicleId,
                Trips = tripsGroupped
            };

        foreach (var trip in tripsGroupped)
        {
            trip.LocationStartName = VehicleLocationHelper.FindNearestLocation(trip.LocationStart, locations)?.Name;
            trip.LocationFinishName = VehicleLocationHelper.FindNearestLocation(trip.LocationFinish, locations)?.Name;
        }

        return new()
        {
            VehicleId = request.VehicleId,
            Trips = tripsGroupped
        };
    }
}