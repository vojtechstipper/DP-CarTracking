using AutoMapper;
using CarTracking.BE.Domain.Entities;

namespace CarTracking.BE.Application.Mappings;

public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<Location, DTOs.LocationDto>();
    }
}