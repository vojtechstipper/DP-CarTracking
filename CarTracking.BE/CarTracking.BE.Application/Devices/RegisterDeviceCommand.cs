using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Domain.Entities;
using CarTracking.BE.Infrastructure;
using MediatR;

namespace CarTracking.BE.Application.Devices;

public class RegisterDeviceCommand : IRequest<RegisteredDeviceDto>
{
    public string AccountId { get; set; } = string.Empty;
    public bool IsAdminDevice { get; set; }
    public string? VehicleId { get; set; }
    public string NotificationToken { get; set; }
}

public class RegisterDeviceCommandHandler(
    CarTrackingDbContext context,
    IVehiclesRepository vehiclesRepository,
    IAccountsRepository accountsRepository,
    IDeviceRepository deviceRepository)
    : IRequestHandler<RegisterDeviceCommand, RegisteredDeviceDto>
{
    public async Task<RegisteredDeviceDto> Handle(RegisterDeviceCommand request, CancellationToken cancellationToken)
    {
          var account = await accountsRepository.GetById(request.AccountId);

        account.ValidateIfNotNull(request.AccountId);

        Device? device = await deviceRepository.GetAdminDeviceByAccountAndToken(account!.Id, request.NotificationToken);

        //Již existuje admin device s tímto tokenem
        if (device is not null) return new RegisteredDeviceDto() { DeviceId = device.Id };

        Vehicle? vehicle = null;
        if (!string.IsNullOrEmpty(request.VehicleId))
        {
            vehicle = await vehiclesRepository.GetById(request.VehicleId);
        }

        device = new Device()
        {
            IsAdminDevice = request.IsAdminDevice,
            NotificationToken = request.NotificationToken,
            Vehicle = vehicle,
            Account = account
        };

        if (vehicle != null)
        {
            vehicle.IsAssignedToDevice = true;
            vehicle.DeviceId = device.Id;
            context.Update(vehicle);
        }

        context.Devices.Add(device);

        await context.SaveChangesAsync(cancellationToken);
        return new RegisteredDeviceDto() { DeviceId = device.Id };
    }
}