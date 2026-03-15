using FleetManagementSystem.Helpers;
using FleetManagementSystem.Models;
using FleetManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FleetManagementSystem.ViewModels
{
    public class VehiculeViewModel : BaseViewModel
    {
        private readonly VehiculeService _service;
        private readonly ClientService _clientService;
        private readonly GarageService _garageService;

        private ObservableCollection<Vehicule> _vehicules = new();
        private ObservableCollection<Client> _clients = new();
        private ObservableCollection<Garage> _garages = new();
        private Vehicule _selectedVehicule = new();

        public ObservableCollection<Vehicule> Vehicules
        {
            get => _vehicules;
            set => SetProperty(ref _vehicules, value);
        }

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        public ObservableCollection<Garage> Garages
        {
            get => _garages;
            set => SetProperty(ref _garages, value);
        }

        public Vehicule SelectedVehicule
        {
            get => _selectedVehicule;
            set => SetProperty(ref _selectedVehicule, value);
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public VehiculeViewModel(VehiculeService service, ClientService clientService, GarageService garageService)
        {
            _service = service;
            _clientService = clientService;
            _garageService = garageService;

            LoadCommand = new RelayCommand(async _ => await LoadDataAsync());
            AddCommand = new RelayCommand(async _ => await AddAsync());
            UpdateCommand = new RelayCommand(async _ => await UpdateAsync(), _ => SelectedVehicule.Id > 0);
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync(), _ => SelectedVehicule.Id > 0);
            ClearCommand = new RelayCommand(_ => ClearSelection());

            // Initial load
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var clientsData = await _clientService.GetAllClientsAsync();
                Clients = new ObservableCollection<Client>(clientsData);

                var garagesData = await _garageService.GetAllGaragesAsync();
                Garages = new ObservableCollection<Garage>(garagesData);

                var vehiculesData = await _service.GetAllVehiculesAsync();
                
                // Map navigation properties
                foreach (var v in vehiculesData)
                {
                    v.Client = clientsData.FirstOrDefault(c => c.Id == v.ClientId);
                    v.Garage = garagesData.FirstOrDefault(g => g.Id == v.GarageId);
                }
                
                Vehicules = new ObservableCollection<Vehicule>(vehiculesData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddAsync()
        {
            try
            {
                SelectedVehicule.Id = 0; // Ensure new
                await _service.AddVehiculeAsync(SelectedVehicule);
                await LoadDataAsync();
                ClearSelection();
                MessageBox.Show("Véhicule ajouté avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task UpdateAsync()
        {
            try
            {
                await _service.UpdateVehiculeAsync(SelectedVehicule);
                await LoadDataAsync();
                ClearSelection();
                MessageBox.Show("Véhicule mis à jour avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task DeleteAsync()
        {
            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce véhicule ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _service.DeleteVehiculeAsync(SelectedVehicule.Id);
                    await LoadDataAsync();
                    ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearSelection()
        {
            SelectedVehicule = new Vehicule();
        }
    }
}
