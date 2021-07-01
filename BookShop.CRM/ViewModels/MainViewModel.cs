using BookShop.CRM.Core.Base;
using BookShop.CRM.ViewModels.Base;
using System;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IUserManager userManager;

        public MainViewModel(IUserManager userManager)
        {
            this.userManager = userManager;
            UserName = $"Hello, {this.userManager.User.UserName}!";
            LogoutCommand = new Command( param => true, ExecuteLogout);
        }

        public Action OpenAuthWindow { get; set; }

        private void ExecuteLogout(object param)
        {
            userManager.User = new();
            OpenAuthWindow();
        }

        private string userName;
        public string UserName 
        {
            get => userName;
            set
            {
                userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public ICommand LogoutCommand { get; }
    }
}
