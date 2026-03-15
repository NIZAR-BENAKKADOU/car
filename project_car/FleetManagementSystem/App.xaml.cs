using FleetManagementSystem.DAO.Implementations;
using FleetManagementSystem.DAO.Interfaces;
using FleetManagementSystem.Data;
using FleetManagementSystem.Services;
using FleetManagementSystem.ViewModels;
using FleetManagementSystem.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace FleetManagementSystem
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Prevent WPF from shutting down when LoginWindow closes
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // ── 1. Build the DI container ────────────────────────────────
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // ── 2. Auto-create database & tables (fixes "does not exist") ─
            try
            {
                var initializer = ServiceProvider.GetRequiredService<DatabaseInitializer>();
                initializer.Initialize();
            }
            catch
            {
                // DatabaseInitializer already showed a MessageBox with details.
                // Shut down gracefully so the user can fix the connection string.
                Shutdown(1);
                return;
            }

            // ── 3. Show LoginWindow first ────────────────────────────────
            var loginVM = ServiceProvider.GetRequiredService<LoginViewModel>();
            var loginWindow = new LoginWindow(loginVM);

            bool? loginResult = loginWindow.ShowDialog();

            if (loginResult != true)
            {
                // User closed the window without logging in
                Shutdown(0);
                return;
            }

            // ── 4. Login succeeded → open MainWindow ─────────────────────
            // Restore normal shutdown behaviour: app exits when MainWindow closes
            ShutdownMode = ShutdownMode.OnMainWindowClose;

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            MainWindow = mainWindow;
            mainWindow.Show();
        }

        // ────────────────────────────────────────────────────────────────
        private static void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // Database
            services.AddSingleton<DatabaseConnection>();
            services.AddSingleton<DatabaseInitializer>();

            // DAOs
            services.AddScoped<IClientDAO, ClientDAO>();
            services.AddScoped<IGarageDAO, GarageDAO>();
            services.AddScoped<IVehiculeDAO, VehiculeDAO>();

            // Services
            services.AddScoped<ClientService>();
            services.AddScoped<GarageService>();
            services.AddScoped<VehiculeService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<ClientViewModel>();
            services.AddTransient<GarageViewModel>();
            services.AddTransient<VehiculeViewModel>();

            // Windows
            services.AddTransient<MainWindow>();
        }
    }
}
