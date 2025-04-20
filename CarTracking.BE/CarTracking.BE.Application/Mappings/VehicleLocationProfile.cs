using AutoMapper;
using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Domain.Entities;

namespace CarTracking.BE.Application.Mappings;

public class VehicleLocationProfile : Profile
{
    public VehicleLocationProfile()
    {
        CreateMap<VehicleLocation, VehicleLocationDto>()
            .ForMember(dest => dest.ToTime,
                opt => opt.MapFrom(src => src.ToTime))
            .ForMember(dest => dest.FromTime,
                opt => opt.MapFrom(src => src.FromTime))
            .ForMember(dest => dest.IsUsedForVirtualGarage,
                opt => opt.MapFrom(src => src.IsUsedForVirtualGarage))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Radius,
                opt => opt.MapFrom(src => src.Radius)).ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                new LocationDto
                {
                    Latitude = src.Latitude,
                    Longitude = src.Longitude
                }));
    }
}