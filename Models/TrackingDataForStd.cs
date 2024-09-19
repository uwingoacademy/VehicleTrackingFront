using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class TrackingDataForStd
    {
        public int TrackingDataId { get; set; }
        [MaxLength(8)]
        public char SerialNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Speed { get; set; }
        public decimal Altitude { get; set; }
        public decimal Odometer { get; set; }
        public bool IsOpenIgnition { get; set; }
        public decimal FuelLevel { get; set; }
        public char NS { get; set; }
        public char EW { get; set; }
        public int Satellites { get; set; }
        public decimal COG { get; set; }
        public bool DIN1 { get; set; }
        public bool DIN2 { get; set; }
        public bool DOUT { get; set; }
        public decimal TotalSpentFuel { get; set; }
        [Column(TypeName = "tinyint")]
        public byte GSM_RSSI { get; set; }
    }
}
