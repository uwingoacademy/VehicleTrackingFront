namespace Frontend.Models
{
    public class DeviceVehicleWithDetails
    {
        public DeviceVehicles DeviceVehicle { get; set; }
        public Vehicles Vehicle { get; set; }
        public Devices Device { get; set; }
    }
}
