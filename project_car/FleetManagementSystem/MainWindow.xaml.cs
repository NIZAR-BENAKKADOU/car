using FleetManagementSystem.ViewModels;
using System.Windows;

namespace FleetManagementSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}