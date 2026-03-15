using FleetManagementSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Input;

namespace FleetManagementSystem.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        private readonly IServiceProvider _serviceProvider;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand NavigateToClientsCommand { get; }
        public ICommand NavigateToGaragesCommand { get; }
        public ICommand NavigateToVehiculesCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            // Navigate to default view
            _currentViewModel = _serviceProvider.GetRequiredService<ClientViewModel>();

            NavigateToClientsCommand = new RelayCommand(o => CurrentViewModel = _serviceProvider.GetRequiredService<ClientViewModel>());
            NavigateToGaragesCommand = new RelayCommand(o => CurrentViewModel = _serviceProvider.GetRequiredService<GarageViewModel>());
            NavigateToVehiculesCommand = new RelayCommand(o => CurrentViewModel = _serviceProvider.GetRequiredService<VehiculeViewModel>());
        }
    }
}
