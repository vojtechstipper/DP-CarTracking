using AutoMapper;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Models;
using Location = CarTracking.MobileApp.Models.Location;

namespace CarTracking.MobileApp.Profiles;

public class VehicleInfoProfile : Profile
{
    public VehicleInfoProfile()
    {
        CreateMap<VehicleListItemDto, VehicleInfo>();
        CreateMap<LocationDto, Location>();
        CreateMap<BatteryInfoDto, BatteryInfo>();
        CreateMap<SignalInfoDto, SignalInfo>();
        CreateMap<VehicleStatusDto, VehicleStatusDetail>();
        CreateMap<VehicleConfigDto, VehicleConfig>();
    }
}