using AutoMapper;
using CarTracking.BE.Application.Vehicles.Queries.VehicleById;
using CarTracking.BE.Domain.Entities;

namespace CarTracking.BE.Application.Mappings;

public class VehicleProfile : Profile
{
    public VehicleProfile()
    {
        CreateMap<Vehicle, VehicleConfigDto>()
            .ForMember(dest => dest.LowBatteryThreshold,
                opt => opt.MapFrom(src => src.Settings.LowBatteryThreshold))
            .ForMember(dest => dest.HistoryInDays,
                opt => opt.MapFrom(src => src.Settings.HistoryLenghtInDays))
            .ForMember(dest => dest.VirtualGarageRadius,
                opt => opt.MapFrom(src => src.Settings.Radius));
    }
}