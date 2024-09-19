namespace Frontend.Models
{
    public class Vehicles
    {
        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string VIN { get; set; }
        public int FirstKilometer { get; set; }
        public string Plate { get; set; }
        public bool IsThereDriver { get; set; }
        public bool? IsItForRent { get; set; }
    }
}
