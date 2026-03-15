using FleetManagementSystem.Helpers;
using FleetManagementSystem.Models;
using FleetManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FleetManagementSystem.ViewModels
{
    public class ClientViewModel : BaseViewModel
    {
        private readonly ClientService _service;
        private ObservableCollection<Client> _clients = new();
        private Client _selectedClient = new();

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set => SetProperty(ref _selectedClient, value);
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public ClientViewModel(ClientService service)
        {
            _service = service;

            LoadCommand = new RelayCommand(async _ => await LoadDataAsync());
            AddCommand = new RelayCommand(async _ => await AddAsync());
            UpdateCommand = new RelayCommand(async _ => await UpdateAsync(), _ => SelectedClient.Id > 0);
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync(), _ => SelectedClient.Id > 0);
            ClearCommand = new RelayCommand(_ => ClearSelection());

            // Initial load
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var data = await _service.GetAllClientsAsync();
                Clients = new ObservableCollection<Client>(data);
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
                SelectedClient.Id = 0; // Ensure it's treated as new
                await _service.AddClientAsync(SelectedClient);
                await LoadDataAsync();
                ClearSelection();
                MessageBox.Show("Client ajouté avec succès.");
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
                await _service.UpdateClientAsync(SelectedClient);
                await LoadDataAsync();
                ClearSelection();
                MessageBox.Show("Client mis à jour avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task DeleteAsync()
        {
            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce client ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _service.DeleteClientAsync(SelectedClient.Id);
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
            SelectedClient = new Client();
        }
    }
}
