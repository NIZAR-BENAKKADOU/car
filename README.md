## 🚗 Fleet Management System


A modern, high-performance **Fleet Management System** built with C# and WPF. This application provides a comprehensive solution for managing vehicles, clients, and maintenance locations with a focus on speed, reliability, and clean architecture.

## 🚀 Features

- **👥 Client Management**: Track client details including contact information and linked vehicles.
- **📍 Garage & Location Tracking**: Manage maintenance garages with GPS coordinate integration.
- **🚘 Vehicle Inventory**: Comprehensive tracking of vehicle registration, manufacturer, and model.
- **🔐 Secure Authentication**: Built-in login system for authorized access.
- **🏗️ Clean Architecture**: Implements MVVM pattern, Service Layer, and DAO for maximum maintainability.
- **🗄️ Database Auto-Initialization**: Automatically sets up the PostgreSQL schema on the first run.

## 🛠️ Technology Stack

- **UI Framework:** Windows Presentation Foundation (WPF)
- **Language:** C# (.NET Core / Framework)
- **Database:** PostgreSQL
- **Pattern:** MVVM (Model-View-ViewModel)
- **ORM/Data Access:** Npgsql with custom DAO implementation

## 📥 Getting Started

### Prerequisites

- Visual Studio 2022 (with .NET Desktop Development workload)
- PostgreSQL Server 14+
- .NET Runtime / SDK compatible with the project files

### Setup & Installation

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/NIZAR-BENAKKADOU/car.git
   cd car/project_car
   ```

2. **Database Configuration:**
   Update the connection string in `FleetManagementSystem/AppSettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "PostgresConnection": "Host=localhost;Port=5432;Database=fleetdb;Username=postgres;Password=YOUR_PASSWORD"
     }
   }
   ```

3. **Build & Run:**
   - Open `project_car.sln` in Visual Studio.
   - Restore NuGet packages.
   - Press `F5` to build and run the application.

> [!NOTE]
> The application will automatically create the `fleetdb` database and all required tables on its first launch.

## 🔑 Default Credentials

- **Username:** `admin`
- **Password:** `admin`

## 📂 Project Structure

- `DAO/`: Data Access Objects for database communication.
- `Models/`: Entity definitions (Client, Vehicle, Garage).
- `Services/`: Business logic layer.
- `ViewModels/`: UI logic following the MVVM pattern.
- `Views/`: XAML-based user interface components.
- `Helpers/`: Utility classes and shared logic.

## 📜 License

This project is licensed under the MIT License.
