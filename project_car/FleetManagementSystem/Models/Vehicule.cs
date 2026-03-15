namespace FleetManagementSystem.Models
{
    public class Vehicule
    {
        public int Id { get; set; }
        public string Matricule { get; set; } = string.Empty;
        public string? Marque { get; set; }
        public string? Modele { get; set; }
        public int? Annee { get; set; }
        public int? ClientId { get; set; }
        public int? GarageId { get; set; }
        
        // Navigation properties (optional but useful for UI bindings)
        public Client? Client { get; set; }
        public Garage? Garage { get; set; }
    }
}
