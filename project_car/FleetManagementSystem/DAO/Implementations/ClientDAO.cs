using FleetManagementSystem.Data;
using FleetManagementSystem.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagementSystem.DAO.Implementations
{
    public class ClientDAO : Interfaces.IClientDAO
    {
        private readonly DatabaseConnection _db;

        public ClientDAO(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            var clients = new List<Client>();
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, cin, nom, prenom, email, telephone FROM clients ORDER BY id", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                clients.Add(new Client
                {
                    Id = reader.GetInt32(0),
                    Cin = reader.GetString(1),
                    Nom = reader.GetString(2),
                    Prenom = reader.GetString(3),
                    Email = reader.GetString(4),
                    Telephone = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }
            return clients;
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, cin, nom, prenom, email, telephone FROM clients WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Client
                {
                    Id = reader.GetInt32(0),
                    Cin = reader.GetString(1),
                    Nom = reader.GetString(2),
                    Prenom = reader.GetString(3),
                    Email = reader.GetString(4),
                    Telephone = reader.IsDBNull(5) ? null : reader.GetString(5)
                };
            }
            return null;
        }

        public async Task<Client?> GetClientByCinAsync(string cin)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, cin, nom, prenom, email, telephone FROM clients WHERE cin = @cin", conn);
            cmd.Parameters.AddWithValue("cin", cin);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Client
                {
                    Id = reader.GetInt32(0),
                    Cin = reader.GetString(1),
                    Nom = reader.GetString(2),
                    Prenom = reader.GetString(3),
                    Email = reader.GetString(4),
                    Telephone = reader.IsDBNull(5) ? null : reader.GetString(5)
                };
            }
            return null;
        }

        public async Task<Client?> GetClientByEmailAsync(string email)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, cin, nom, prenom, email, telephone FROM clients WHERE email = @email", conn);
            cmd.Parameters.AddWithValue("email", email);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Client
                {
                    Id = reader.GetInt32(0),
                    Cin = reader.GetString(1),
                    Nom = reader.GetString(2),
                    Prenom = reader.GetString(3),
                    Email = reader.GetString(4),
                    Telephone = reader.IsDBNull(5) ? null : reader.GetString(5)
                };
            }
            return null;
        }

        public async Task AddClientAsync(Client client)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("INSERT INTO clients (cin, nom, prenom, email, telephone) VALUES (@cin, @nom, @prenom, @email, @telephone)", conn);
            cmd.Parameters.AddWithValue("cin", client.Cin);
            cmd.Parameters.AddWithValue("nom", client.Nom);
            cmd.Parameters.AddWithValue("prenom", client.Prenom);
            cmd.Parameters.AddWithValue("email", client.Email);
            cmd.Parameters.AddWithValue("telephone", string.IsNullOrEmpty(client.Telephone) ? DBNull.Value : client.Telephone);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateClientAsync(Client client)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("UPDATE clients SET cin = @cin, nom = @nom, prenom = @prenom, email = @email, telephone = @telephone WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", client.Id);
            cmd.Parameters.AddWithValue("cin", client.Cin);
            cmd.Parameters.AddWithValue("nom", client.Nom);
            cmd.Parameters.AddWithValue("prenom", client.Prenom);
            cmd.Parameters.AddWithValue("email", client.Email);
            cmd.Parameters.AddWithValue("telephone", string.IsNullOrEmpty(client.Telephone) ? DBNull.Value : client.Telephone);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("DELETE FROM clients WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
