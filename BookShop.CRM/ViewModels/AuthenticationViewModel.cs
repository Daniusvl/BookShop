using BookShop.CRM.Core;
using BookShop.CRM.Core.Base;
using BookShop.CRM.Core.Models;
using BookShop.CRM.ViewModels.Base;
using BookShop.CRM.Wrappers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            authentication = new(new());
            LoginCommand = new Command(CanLogin, Login);

            authentication.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName == nameof(authentication.HasErrors))
                    ((Command)LoginCommand).RaiseCanExecuteChanged();
            };
            ((Command)LoginCommand).RaiseCanExecuteChanged();
            this.authenticationService = authenticationService;
        }

        public Action OpenMainWindow { get; set; }

        private bool CanLogin(object param)
        {
            return !authentication.HasErrors;
        }

        private async void Login(object param) 
        {
            ButtonEnabled = false;

            AuthenticatedUser user = await authenticationService.Authenticate(new AuthenticateCommand(authentication.Email, authentication.Password));
            if(user == null)
            {
                MessageBox.Show("Email or password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                OpenMainWindow();
            }

            ButtonEnabled = true;
        }

        public async Task Load()
        {
            bool result = await authenticationService.TryAuthenticate();
            if (result)
            {
                OpenMainWindow();
            }
        }

        private IAuthenticationService authenticationService;
        private AuthenticationWrapper authentication;
        public AuthenticationWrapper Authentication
        {
            get => authentication;
            set
            {
                authentication = value;
                OnPropertyChanged(nameof(Authentication));
            }
        }

        private bool button_enabled = true;
        public bool ButtonEnabled
        {
            get => button_enabled;
            set
            {
                button_enabled = value;
                OnPropertyChanged(nameof(ButtonEnabled));
            }
        }


        public ICommand LoginCommand { get; }
    }
}
