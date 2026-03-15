-- =============================================================================
--  Fleet Management System — PostgreSQL Database Schema
-- =============================================================================
--
--  NOTE: As of the latest version, the application automatically creates the
--  database and all tables on first startup via DatabaseInitializer.cs.
--  You do NOT need to run this script manually unless you want to reset or
--  inspect the schema outside of the application.
--
--  Manual setup (optional):
--  -------------------------
--  1. Connect to PostgreSQL as a superuser (e.g. postgres)
--  2. Run:  CREATE DATABASE fleetdb ENCODING 'UTF8';
--  3. Connect to fleetdb:  \c fleetdb
--  4. Run the CREATE TABLE statements below.
--
--  Connection settings are stored in:
--      FleetManagementSystem/AppSettings.json
--
--  Default credentials (application login):
--      Username : admin
--      Password : admin
-- =============================================================================


-- -----------------------------------------------------------------------------
--  Create target database (run as superuser, outside any database context)
-- -----------------------------------------------------------------------------
-- CREATE DATABASE fleetdb
--     ENCODING    = 'UTF8'
--     LC_COLLATE  = 'en_US.UTF-8'
--     LC_CTYPE    = 'en_US.UTF-8'
--     TEMPLATE    = template0;


-- =============================================================================
--  Connect to fleetdb before running the rest
-- =============================================================================
-- \c fleetdb


-- =============================================================================
--  TABLE: clients
-- =============================================================================
CREATE TABLE IF NOT EXISTS clients (
    id        SERIAL       PRIMARY KEY,
    cin       VARCHAR(20)  UNIQUE NOT NULL,
    nom       VARCHAR(100) NOT NULL,
    prenom    VARCHAR(100) NOT NULL,
    email     VARCHAR(150) UNIQUE NOT NULL,
    telephone VARCHAR(20)
);

COMMENT ON TABLE  clients           IS 'Company clients who own or rent vehicles.';
COMMENT ON COLUMN clients.cin       IS 'National ID card number — must be unique.';
COMMENT ON COLUMN clients.email     IS 'Contact e-mail address — must be unique.';
COMMENT ON COLUMN clients.telephone IS 'Optional phone number.';


-- =============================================================================
--  TABLE: garages
-- =============================================================================
CREATE TABLE IF NOT EXISTS garages (
    id        SERIAL         PRIMARY KEY,
    nom       VARCHAR(150)   NOT NULL,
    longitude DOUBLE PRECISION,
    latitude  DOUBLE PRECISION
);

COMMENT ON TABLE  garages           IS 'Garages / parking locations used by the fleet.';
COMMENT ON COLUMN garages.nom       IS 'Display name of the garage.';
COMMENT ON COLUMN garages.longitude IS 'GPS longitude coordinate.';
COMMENT ON COLUMN garages.latitude  IS 'GPS latitude coordinate.';


-- =============================================================================
--  TABLE: vehicules
-- =============================================================================
CREATE TABLE IF NOT EXISTS vehicules (
    id         SERIAL      PRIMARY KEY,
    matricule  VARCHAR(20) UNIQUE NOT NULL,
    marque     VARCHAR(100),
    modele     VARCHAR(100),
    annee      INT,
    client_id  INT REFERENCES clients(id) ON DELETE SET NULL,
    garage_id  INT REFERENCES garages(id) ON DELETE SET NULL
);

COMMENT ON TABLE  vehicules            IS 'Fleet vehicles tracked by the system.';
COMMENT ON COLUMN vehicules.matricule  IS 'Vehicle registration plate — must be unique.';
COMMENT ON COLUMN vehicules.marque     IS 'Manufacturer brand (e.g. Renault, Toyota).';
COMMENT ON COLUMN vehicules.modele     IS 'Model name (e.g. Clio, Corolla).';
COMMENT ON COLUMN vehicules.annee      IS 'Year of manufacture.';
COMMENT ON COLUMN vehicules.client_id  IS 'FK → clients.id  (SET NULL on client deletion).';
COMMENT ON COLUMN vehicules.garage_id  IS 'FK → garages.id  (SET NULL on garage deletion).';


-- =============================================================================
--  Sample seed data (optional — comment out if not needed)
-- =============================================================================

-- INSERT INTO clients (cin, nom, prenom, email, telephone) VALUES
--     ('AB123456', 'Dupont',  'Jean',   'jean.dupont@example.com',  '+212600000001'),
--     ('CD789012', 'Martin',  'Sophie', 'sophie.martin@example.com','+212600000002'),
--     ('EF345678', 'Benali',  'Youssef','y.benali@example.com',     '+212600000003');

-- INSERT INTO garages (nom, longitude, latitude) VALUES
--     ('Garage Central',    -7.5898,  33.5731),
--     ('Garage Nord',       -7.6114,  33.6000),
--     ('Garage Sud',        -7.5700,  33.5400);

-- INSERT INTO vehicules (matricule, marque, modele, annee, client_id, garage_id) VALUES
--     ('12345-A-1',  'Renault', 'Clio',    2020, 1, 1),
--     ('67890-B-2',  'Toyota',  'Corolla', 2019, 2, 2),
--     ('11111-C-3',  'Peugeot', '208',     2021, 3, 3);


-- =============================================================================
--  Useful diagnostic queries
-- =============================================================================

-- List all vehicles with their client and garage names:
-- SELECT v.id, v.matricule, v.marque, v.modele, v.annee,
--        c.nom || ' ' || c.prenom AS client,
--        g.nom                    AS garage
-- FROM   vehicules v
-- LEFT JOIN clients c ON c.id = v.client_id
-- LEFT JOIN garages g ON g.id = v.garage_id
-- ORDER BY v.id;
