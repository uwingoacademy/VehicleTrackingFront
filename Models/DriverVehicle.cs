namespace Frontend.Models
{
    public class DriverVehicle
    {
        public int DriverVehicleId { get; set; }
        public int DriversId { get; set; }
        public int VehicleId { get; set; }
        public DateTime IdentificationDate { get; set; }
        public DateTime? TerminationDate { get; set; }
    }
}
