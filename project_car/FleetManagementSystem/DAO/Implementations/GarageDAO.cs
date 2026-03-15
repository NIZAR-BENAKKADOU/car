using FleetManagementSystem.Data;
using FleetManagementSystem.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagementSystem.DAO.Implementations
{
    public class GarageDAO : Interfaces.IGarageDAO
    {
        private readonly DatabaseConnection _db;

        public GarageDAO(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<List<Garage>> GetAllGaragesAsync()
        {
            var garages = new List<Garage>();
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, nom, longitude, latitude FROM garages ORDER BY id", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                garages.Add(new Garage
                {
                    Id = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Longitude = reader.IsDBNull(2) ? null : reader.GetDouble(2),
                    Latitude = reader.IsDBNull(3) ? null : reader.GetDouble(3)
                });
            }
            return garages;
        }

        public async Task<Garage?> GetGarageByIdAsync(int id)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, nom, longitude, latitude FROM garages WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Garage
                {
                    Id = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Longitude = reader.IsDBNull(2) ? null : reader.GetDouble(2),
                    Latitude = reader.IsDBNull(3) ? null : reader.GetDouble(3)
                };
            }
            return null;
        }

        public async Task AddGarageAsync(Garage garage)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("INSERT INTO garages (nom, longitude, latitude) VALUES (@nom, @longitude, @latitude)", conn);
            cmd.Parameters.AddWithValue("nom", garage.Nom);
            cmd.Parameters.AddWithValue("longitude", garage.Longitude.HasValue ? garage.Longitude.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("latitude", garage.Latitude.HasValue ? garage.Latitude.Value : DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateGarageAsync(Garage garage)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("UPDATE garages SET nom = @nom, longitude = @longitude, latitude = @latitude WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", garage.Id);
            cmd.Parameters.AddWithValue("nom", garage.Nom);
            cmd.Parameters.AddWithValue("longitude", garage.Longitude.HasValue ? garage.Longitude.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("latitude", garage.Latitude.HasValue ? garage.Latitude.Value : DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteGarageAsync(int id)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("DELETE FROM garages WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
