using FleetManagementSystem.DAO.Interfaces;
using FleetManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagementSystem.Services
{
    public class VehiculeService
    {
        private readonly IVehiculeDAO _vehiculeDAO;

        public VehiculeService(IVehiculeDAO vehiculeDAO)
        {
            _vehiculeDAO = vehiculeDAO;
        }

        public async Task<List<Vehicule>> GetAllVehiculesAsync()
        {
            return await _vehiculeDAO.GetAllVehiculesAsync();
        }

        public async Task AddVehiculeAsync(Vehicule vehicule)
        {
            await ValidateVehiculeAsync(vehicule);
            await _vehiculeDAO.AddVehiculeAsync(vehicule);
        }

        public async Task UpdateVehiculeAsync(Vehicule vehicule)
        {
            await ValidateVehiculeAsync(vehicule, isUpdate: true);
            await _vehiculeDAO.UpdateVehiculeAsync(vehicule);
        }

        public async Task DeleteVehiculeAsync(int id)
        {
            await _vehiculeDAO.DeleteVehiculeAsync(id);
        }

        private async Task ValidateVehiculeAsync(Vehicule vehicule, bool isUpdate = false)
        {
            if (string.IsNullOrWhiteSpace(vehicule.Matricule))
                throw new ArgumentException("La matricule est requise.");
            if (!vehicule.ClientId.HasValue)
                throw new ArgumentException("Le véhicule doit être lié à un client.");
            if (!vehicule.GarageId.HasValue)
                throw new ArgumentException("Le véhicule doit être lié à un garage.");

            var existingVehicule = await _vehiculeDAO.GetVehiculeByMatriculeAsync(vehicule.Matricule);
            if (existingVehicule != null && (!isUpdate || existingVehicule.Id != vehicule.Id))
            {
                throw new InvalidOperationException("Un véhicule avec cette matricule existe déjà.");
            }
        }
    }
}
