namespace Frontend.Models
{
    public class PeriodicMaintenance
    {
        public int PeriodicMaintenanceId { get; set; }
        public int VehicleId { get; set; }
        public int Period { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public int Days { 
            get {
                if (LastMaintenanceDate != null && NextMaintenanceDate != null)
                {
                    TimeSpan difference = NextMaintenanceDate.Value - DateTime.Now;
                    return difference.Days;
                }
                return 0;
            }
        }
    }
}
