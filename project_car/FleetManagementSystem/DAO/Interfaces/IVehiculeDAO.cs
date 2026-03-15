using FleetManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagementSystem.DAO.Interfaces
{
    public interface IVehiculeDAO
    {
        Task<List<Vehicule>> GetAllVehiculesAsync();
        Task<Vehicule?> GetVehiculeByIdAsync(int id);
        Task<Vehicule?> GetVehiculeByMatriculeAsync(string matricule);
        Task AddVehiculeAsync(Vehicule vehicule);
        Task UpdateVehiculeAsync(Vehicule vehicule);
        Task DeleteVehiculeAsync(int id);
    }
}
