namespace FleetManagementSystem.Models
{
    public class Garage
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }
}
