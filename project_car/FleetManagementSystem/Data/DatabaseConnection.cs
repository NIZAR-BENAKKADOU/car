using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace FleetManagementSystem.Data
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresConnection") 
                ?? throw new System.ArgumentNullException("PostgresConnection string is not defined in AppSettings.json");
        }

        public NpgsqlConnection GetConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            return connection;
        }
    }
}
