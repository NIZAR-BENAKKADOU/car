using FleetManagementSystem.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace FleetManagementSystem.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        // ── Close button ────────────────────────────────────────────────
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // ── Drag the borderless window by its header ─────────────────────
        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        // ── Enter key in any input field triggers login ──────────────────
        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AttemptLogin();
        }

        // ── Login button click ───────────────────────────────────────────
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        // ── Core login logic ─────────────────────────────────────────────
        private void AttemptLogin()
        {
            // Clear previous error
            ErrorText.Visibility = Visibility.Collapsed;
            ErrorText.Text = string.Empty;

            // Retrieve the password from the PasswordBox (not bindable for security)
            _viewModel.Password = PasswordBox.Password;

            bool success = false;

            // Subscribe temporarily to know whether login succeeded
            _viewModel.LoginSucceeded = () =>
            {
                success = true;
            };

            _viewModel.LoginFailed = (message) =>
            {
                ErrorText.Text = message;
                ErrorText.Visibility = Visibility.Visible;

                // Shake the window for visual feedback
                ShakeWindow();
            };

            _viewModel.ExecuteLogin();

            if (success)
            {
                DialogResult = true;
                Close();
            }
        }

        // ── Simple shake animation on bad credentials ────────────────────
        private async void ShakeWindow()
        {
            double originalLeft = Left;
            int[] offsets = { -8, 8, -6, 6, -4, 4, -2, 2, 0 };
            foreach (int offset in offsets)
            {
                Left = originalLeft + offset;
                await System.Threading.Tasks.Task.Delay(30);
            }
            Left = originalLeft;
        }
    }
}
