namespace Frontend.Models
{
    public class DeviceVehicles
    {
        public int ConnectionId { get; set; }
        public int DeviceId { get; set; }
        public int VehicleId { get; set; }
        public DateTime? InstallDate { get; set; }
        public DateTime? RemoveDate { get; set; }
    }
}
