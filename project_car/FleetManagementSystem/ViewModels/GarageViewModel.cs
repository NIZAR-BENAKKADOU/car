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
    public class GarageViewModel : BaseViewModel
    {
        private readonly GarageService _service;
        private ObservableCollection<Garage> _garages = new();
        private Garage _selectedGarage = new();

        public ObservableCollection<Garage> Garages
        {
            get => _garages;
            set => SetProperty(ref _garages, value);
        }

        public Garage SelectedGarage
        {
            get => _selectedGarage;
            set => SetProperty(ref _selectedGarage, value);
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public GarageViewModel(GarageService service)
        {
            _service = service;

            LoadCommand = new RelayCommand(async _ => await LoadDataAsync());
            AddCommand = new RelayCommand(async _ => await AddAsync());
            UpdateCommand = new RelayCommand(async _ => await UpdateAsync(), _ => SelectedGarage.Id > 0);
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync(), _ => SelectedGarage.Id > 0);
            ClearCommand = new RelayCommand(_ => ClearSelection());

            // Initial load
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var data = await _service.GetAllGaragesAsync();
                Garages = new ObservableCollection<Garage>(data);
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
                SelectedGarage.Id = 0; // Ensure new
                await _service.AddGarageAsync(SelectedGarage);
                await LoadDataAsync();
                ClearSelection();
                MessageBox.Show("Garage ajouté avec succès.");
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
                await _service.UpdateGarageAsync(SelectedGarage);
                await LoadDataAsync();
                ClearSelection();
                MessageBox.Show("Garage mis à jour avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task DeleteAsync()
        {
            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce garage ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _service.DeleteGarageAsync(SelectedGarage.Id);
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
            SelectedGarage = new Garage();
        }
    }
}
