using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Application.Services;
using MediatR;

namespace CarTracking.BE.Application.Devices;

public class TestNotificationFromDeviceCommand : IRequest<Unit>
{
    public required string AccountId { get; init; }
    public required string DeviceId { get; init; }
}

public class TestNotificationFromDeviceCommandHandler(
    INotificationSender notificationSender,
    IDeviceRepository deviceRepository) : IRequestHandler<TestNotificationFromDeviceCommand, Unit>
{
    public async Task<Unit> Handle(TestNotificationFromDeviceCommand request, CancellationToken cancellationToken)
    {
        var adminDevices = (await deviceRepository.GetAdminDevicesForAccount(request.AccountId))
            .Select(x => x.NotificationToken).ToList();
        var device = await deviceRepository.GetDeviceByIdWithVehicle(request.DeviceId);
        device.ValidateIfNotNull(request.DeviceId);

        var notificationMessage = new NotificationDto()
        {
            Title = $"Test notification from device for vehicle {device!.Vehicle?.Name}",
            Body =
                "Manually triggered test notification",
            Data = new()
        };

        if (adminDevices.Any())
            await notificationSender.SendNotificationsAsync(notificationMessage, adminDevices);
        return Unit.Value;
    }
}