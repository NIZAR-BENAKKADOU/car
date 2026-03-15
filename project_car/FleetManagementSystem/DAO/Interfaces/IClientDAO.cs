using FleetManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagementSystem.DAO.Interfaces
{
    public interface IClientDAO
    {
        Task<List<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client?> GetClientByCinAsync(string cin);
        Task<Client?> GetClientByEmailAsync(string email);
        Task AddClientAsync(Client client);
        Task UpdateClientAsync(Client client);
        Task DeleteClientAsync(int id);
    }
}
