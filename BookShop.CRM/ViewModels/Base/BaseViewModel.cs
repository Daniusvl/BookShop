using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookShop.CRM.ViewModels.Base
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = default)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}
