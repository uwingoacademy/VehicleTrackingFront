namespace Frontend.Models
{
    public class Devices
    {
        public int DeviceId { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string PacketType { get; set; }
        public bool IsConnectedVehicles { get; set; }
    }
}
