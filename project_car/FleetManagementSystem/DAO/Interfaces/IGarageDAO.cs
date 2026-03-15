using FleetManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagementSystem.DAO.Interfaces
{
    public interface IGarageDAO
    {
        Task<List<Garage>> GetAllGaragesAsync();
        Task<Garage?> GetGarageByIdAsync(int id);
        Task AddGarageAsync(Garage garage);
        Task UpdateGarageAsync(Garage garage);
        Task DeleteGarageAsync(int id);
    }
}
