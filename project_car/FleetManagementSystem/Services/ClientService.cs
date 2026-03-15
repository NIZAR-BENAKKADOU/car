using FleetManagementSystem.DAO.Interfaces;
using FleetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagementSystem.Services
{
    public class ClientService
    {
        private readonly IClientDAO _clientDAO;

        public ClientService(IClientDAO clientDAO)
        {
            _clientDAO = clientDAO;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _clientDAO.GetAllClientsAsync();
        }

        public async Task AddClientAsync(Client client)
        {
            await ValidateClientAsync(client);
            await _clientDAO.AddClientAsync(client);
        }

        public async Task UpdateClientAsync(Client client)
        {
            await ValidateClientAsync(client, isUpdate: true);
            await _clientDAO.UpdateClientAsync(client);
        }

        public async Task DeleteClientAsync(int id)
        {
            await _clientDAO.DeleteClientAsync(id);
        }

        private async Task ValidateClientAsync(Client client, bool isUpdate = false)
        {
            if (string.IsNullOrWhiteSpace(client.Cin))
                throw new ArgumentException("CIN is required.");
            if (string.IsNullOrWhiteSpace(client.Nom))
                throw new ArgumentException("Nom is required.");
            if (string.IsNullOrWhiteSpace(client.Prenom))
                throw new ArgumentException("Prenom is required.");
            if (string.IsNullOrWhiteSpace(client.Email))
                throw new ArgumentException("Email is required.");

            var existingCin = await _clientDAO.GetClientByCinAsync(client.Cin);
            if (existingCin != null && (!isUpdate || existingCin.Id != client.Id))
            {
                throw new InvalidOperationException("Un client avec ce CIN existe déjà.");
            }

            var existingEmail = await _clientDAO.GetClientByEmailAsync(client.Email);
            if (existingEmail != null && (!isUpdate || existingEmail.Id != client.Id))
            {
                throw new InvalidOperationException("Un client avec cet Email existe déjà.");
            }
        }
    }
}
