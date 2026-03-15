using FleetManagementSystem.Data;
using FleetManagementSystem.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagementSystem.DAO.Implementations
{
    public class VehiculeDAO : Interfaces.IVehiculeDAO
    {
        private readonly DatabaseConnection _db;

        public VehiculeDAO(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<List<Vehicule>> GetAllVehiculesAsync()
        {
            var vehicules = new List<Vehicule>();
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, matricule, marque, modele, annee, client_id, garage_id FROM vehicules ORDER BY id", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                vehicules.Add(new Vehicule
                {
                    Id = reader.GetInt32(0),
                    Matricule = reader.GetString(1),
                    Marque = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Modele = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Annee = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    ClientId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    GarageId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                });
            }
            return vehicules;
        }

        public async Task<Vehicule?> GetVehiculeByIdAsync(int id)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, matricule, marque, modele, annee, client_id, garage_id FROM vehicules WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Vehicule
                {
                    Id = reader.GetInt32(0),
                    Matricule = reader.GetString(1),
                    Marque = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Modele = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Annee = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    ClientId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    GarageId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                };
            }
            return null;
        }

        public async Task<Vehicule?> GetVehiculeByMatriculeAsync(string matricule)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, matricule, marque, modele, annee, client_id, garage_id FROM vehicules WHERE matricule = @matricule", conn);
            cmd.Parameters.AddWithValue("matricule", matricule);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Vehicule
                {
                    Id = reader.GetInt32(0),
                    Matricule = reader.GetString(1),
                    Marque = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Modele = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Annee = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    ClientId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    GarageId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                };
            }
            return null;
        }

        public async Task AddVehiculeAsync(Vehicule vehicule)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("INSERT INTO vehicules (matricule, marque, modele, annee, client_id, garage_id) VALUES (@matricule, @marque, @modele, @annee, @client_id, @garage_id)", conn);
            cmd.Parameters.AddWithValue("matricule", vehicule.Matricule);
            cmd.Parameters.AddWithValue("marque", string.IsNullOrEmpty(vehicule.Marque) ? DBNull.Value : vehicule.Marque);
            cmd.Parameters.AddWithValue("modele", string.IsNullOrEmpty(vehicule.Modele) ? DBNull.Value : vehicule.Modele);
            cmd.Parameters.AddWithValue("annee", vehicule.Annee.HasValue ? vehicule.Annee.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("client_id", vehicule.ClientId.HasValue ? vehicule.ClientId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("garage_id", vehicule.GarageId.HasValue ? vehicule.GarageId.Value : DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateVehiculeAsync(Vehicule vehicule)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("UPDATE vehicules SET matricule = @matricule, marque = @marque, modele = @modele, annee = @annee, client_id = @client_id, garage_id = @garage_id WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", vehicule.Id);
            cmd.Parameters.AddWithValue("matricule", vehicule.Matricule);
            cmd.Parameters.AddWithValue("marque", string.IsNullOrEmpty(vehicule.Marque) ? DBNull.Value : vehicule.Marque);
            cmd.Parameters.AddWithValue("modele", string.IsNullOrEmpty(vehicule.Modele) ? DBNull.Value : vehicule.Modele);
            cmd.Parameters.AddWithValue("annee", vehicule.Annee.HasValue ? vehicule.Annee.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("client_id", vehicule.ClientId.HasValue ? vehicule.ClientId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("garage_id", vehicule.GarageId.HasValue ? vehicule.GarageId.Value : DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteVehiculeAsync(int id)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("DELETE FROM vehicules WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
