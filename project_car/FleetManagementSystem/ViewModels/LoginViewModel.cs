using FleetManagementSystem.Helpers;
using System;
using System.Windows.Input;

namespace FleetManagementSystem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        // ── Bindable Properties ──────────────────────────────────────────
        private string _username = string.Empty;
        private string _password = string.Empty;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        /// <summary>
        /// Password is set directly by the code-behind from the PasswordBox
        /// (PasswordBox is not bindable for security reasons).
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        // ── Callbacks set by the View before calling ExecuteLogin ────────
        /// <summary>Invoked when credentials are correct.</summary>
        public Action? LoginSucceeded { get; set; }

        /// <summary>Invoked with an error message when credentials are wrong.</summary>
        public Action<string>? LoginFailed { get; set; }

        // ── ICommand (kept for optional XAML binding) ────────────────────
        public ICommand LoginCommand { get; }

        // ── Constructor ──────────────────────────────────────────────────
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(
                _ => ExecuteLogin(),
                _ => !string.IsNullOrWhiteSpace(Username));
        }

        // ── Core Authentication ──────────────────────────────────────────
        /// <summary>
        /// Validates credentials and fires either LoginSucceeded or LoginFailed.
        /// Credentials are hardcoded as admin / admin for this demonstration.
        /// </summary>
        public void ExecuteLogin()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                LoginFailed?.Invoke("Le nom d'utilisateur est obligatoire.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                LoginFailed?.Invoke("Le mot de passe est obligatoire.");
                return;
            }

            if (Username.Trim() == "admin" && Password == "admin")
            {
                LoginSucceeded?.Invoke();
            }
            else
            {
                LoginFailed?.Invoke("Nom d'utilisateur ou mot de passe incorrect.");
            }
        }
    }
}
