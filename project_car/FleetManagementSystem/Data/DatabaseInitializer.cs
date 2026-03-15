using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Windows;

namespace FleetManagementSystem.Data
{
    public class DatabaseInitializer
    {
        private readonly IConfiguration _configuration;
        private readonly string _fullConnectionString;

        public DatabaseInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
            _fullConnectionString = _configuration.GetConnectionString("PostgresConnection")
                ?? throw new ArgumentNullException("PostgresConnection string is not defined in AppSettings.json");
        }

        /// <summary>
        /// Ensures the database and all required tables exist.
        /// Connects to 'postgres' first to create the target DB if missing,
        /// then connects to the target DB and creates tables.
        /// </summary>
        public void Initialize()
        {
            try
            {
                EnsureDatabaseExists();
                EnsureTablesExist();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur d'initialisation de la base de données :\n\n{ex.Message}\n\n" +
                    "Vérifiez que PostgreSQL est démarré et que les paramètres de connexion dans AppSettings.json sont corrects.",
                    "Erreur de base de données",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }

        // ---------------------------------------------------------------
        // Step 1 – Make sure the database itself exists
        // ---------------------------------------------------------------
        private void EnsureDatabaseExists()
        {
            var builder = new NpgsqlConnectionStringBuilder(_fullConnectionString);
            string targetDatabase = builder.Database ?? "fleetdb";

            // Connect to the maintenance database to check / create the target DB
            builder.Database = "postgres";
            string maintenanceConnStr = builder.ToString();

            using var conn = new NpgsqlConnection(maintenanceConnStr);
            conn.Open();

            // Check whether the DB already exists
            using var checkCmd = new NpgsqlCommand(
                "SELECT 1 FROM pg_database WHERE datname = @dbname", conn);
            checkCmd.Parameters.AddWithValue("@dbname", targetDatabase);

            var result = checkCmd.ExecuteScalar();

            if (result == null)
            {
                // CREATE DATABASE cannot run inside a transaction, so we use
                // NpgsqlConnection.ExecuteNonQuery which is outside a transaction.
                using var createCmd = new NpgsqlCommand(
                    $"CREATE DATABASE \"{targetDatabase}\" ENCODING 'UTF8'", conn);
                createCmd.ExecuteNonQuery();
            }
        }

        // ---------------------------------------------------------------
        // Step 2 – Make sure every table exists inside the target database
        // ---------------------------------------------------------------
        private void EnsureTablesExist()
        {
            using var conn = new NpgsqlConnection(_fullConnectionString);
            conn.Open();

            // ── clients ─────────────────────────────────────────────────
            ExecuteNonQuery(conn, @"
                CREATE TABLE IF NOT EXISTS clients (
                    id        SERIAL PRIMARY KEY,
                    cin       VARCHAR(20)  UNIQUE NOT NULL,
                    nom       VARCHAR(100) NOT NULL,
                    prenom    VARCHAR(100) NOT NULL,
                    email     VARCHAR(150) UNIQUE NOT NULL,
                    telephone VARCHAR(20)
                );");

            // ── garages ─────────────────────────────────────────────────
            ExecuteNonQuery(conn, @"
                CREATE TABLE IF NOT EXISTS garages (
                    id        SERIAL PRIMARY KEY,
                    nom       VARCHAR(150) NOT NULL,
                    longitude DOUBLE PRECISION,
                    latitude  DOUBLE PRECISION
                );");

            // ── vehicules ────────────────────────────────────────────────
            ExecuteNonQuery(conn, @"
                CREATE TABLE IF NOT EXISTS vehicules (
                    id         SERIAL PRIMARY KEY,
                    matricule  VARCHAR(20)  UNIQUE NOT NULL,
                    marque     VARCHAR(100),
                    modele     VARCHAR(100),
                    annee      INT,
                    client_id  INT REFERENCES clients(id)  ON DELETE SET NULL,
                    garage_id  INT REFERENCES garages(id)  ON DELETE SET NULL
                );");
        }

        // ---------------------------------------------------------------
        // Helper
        // ---------------------------------------------------------------
        private static void ExecuteNonQuery(NpgsqlConnection conn, string sql)
        {
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}
