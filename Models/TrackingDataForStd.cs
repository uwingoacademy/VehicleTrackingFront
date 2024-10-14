using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class TrackingDataForStd
    {
        public int? TrackingDataId { get; set; }
        [MaxLength(8)]
        public string SerialNumber { get; set; }
        public DateTime dateTime { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Speed { get; set; }
        public float Altitude { get; set; }
        public float OdoMeter { get; set; }
        public bool Workingstatus { get; set; }
        public float FuelLevel { get; set; }
        public string NorthSouth { get; set; }
        public string EastWest { get; set; }
        public int Satellites { get; set; }
        public float COG { get; set; }
        public bool Input1Status { get; set; }
        public bool Input2Status { get; set; }
        public bool OutPutStatus { get; set; }
        public float TotalSpentFuel { get; set; }
        [Column(TypeName = "tinyint")]
        public int Numberofsatellites { get; set; }
    }
}
