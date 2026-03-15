namespace FleetManagementSystem.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Cin { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telephone { get; set; }
    }
}
