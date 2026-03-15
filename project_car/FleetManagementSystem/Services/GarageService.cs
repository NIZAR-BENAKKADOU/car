using FleetManagementSystem.DAO.Interfaces;
using FleetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagementSystem.Services
{
    public class GarageService
    {
        private readonly IGarageDAO _garageDAO;

        public GarageService(IGarageDAO garageDAO)
        {
            _garageDAO = garageDAO;
        }

        public async Task<List<Garage>> GetAllGaragesAsync()
        {
            return await _garageDAO.GetAllGaragesAsync();
        }

        public async Task AddGarageAsync(Garage garage)
        {
            ValidateGarage(garage);
            await _garageDAO.AddGarageAsync(garage);
        }

        public async Task UpdateGarageAsync(Garage garage)
        {
            ValidateGarage(garage);
            await _garageDAO.UpdateGarageAsync(garage);
        }

        public async Task DeleteGarageAsync(int id)
        {
            await _garageDAO.DeleteGarageAsync(id);
        }

        private void ValidateGarage(Garage garage)
        {
            if (string.IsNullOrWhiteSpace(garage.Nom))
                throw new ArgumentException("Le nom du garage est requis.");
            if (!garage.Latitude.HasValue)
                throw new ArgumentException("La latitude est requise.");
            if (!garage.Longitude.HasValue)
                throw new ArgumentException("La longitude est requise.");
        }
    }
}
