namespace CarTracking.MobileApp.DTOs;

public class AddNewVehicleCommand
{
    public string Name { get; set; }

    public AddNewVehicleCommand(string vehicleName)
    {
        Name = vehicleName;
    }
}