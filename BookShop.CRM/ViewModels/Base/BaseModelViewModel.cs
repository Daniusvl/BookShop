using BookShop.CRM.Core.Exceptions;
using BookShop.CRM.Wrappers.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels.Base
{
    public abstract class BaseModelViewModel<TCollection, TSelectedItem> : BaseViewModel
        where TSelectedItem : ModelWrapper<TCollection>
    {
        protected virtual async Task SafeRequestSend(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected List<TCollection> collection;
        public virtual List<TCollection> Collection
        {
            get => collection;
            set
            {
                collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        protected TSelectedItem selectedItem;
        public virtual TSelectedItem SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public virtual ICommand LoadCommand { get; protected set; }

        public virtual ICommand AddCommand { get; protected set; }

        public virtual ICommand UpdateCommand { get; protected set; }

        public virtual ICommand DeleteCommand { get; protected set; }


        protected bool deleteEnabled = true;
        public virtual bool DeleteEnabled
        {
            get => deleteEnabled;
            set
            {
                deleteEnabled = value;
                OnPropertyChanged(nameof(DeleteEnabled));
            }
        }

        protected bool updateEnabled = true;
        public virtual bool UpdateEnabled
        {
            get => updateEnabled;
            set
            {
                updateEnabled = value;
                OnPropertyChanged(nameof(UpdateEnabled));
            }
        }

        protected bool addEnabled = true;
        public virtual bool AddEnabled
        {
            get => addEnabled;
            set
            {
                addEnabled = value;
                OnPropertyChanged(nameof(AddEnabled));
            }
        }

        protected bool loadEnabled = true;
        public virtual bool LoadEnabled
        {
            get => loadEnabled;
            set
            {
                loadEnabled = value;
                OnPropertyChanged(nameof(LoadEnabled));
            }
        }

        protected Visibility deleteVisibility = Visibility.Visible;
        public virtual Visibility DeleteVisibility
        {
            get => deleteVisibility;
            set
            {
                deleteVisibility = value;
                OnPropertyChanged(nameof(DeleteVisibility));
            }
        }

        protected Visibility updateVisibility = Visibility.Visible;
        public virtual Visibility UpdateVisibility
        {
            get => updateVisibility;
            set
            {
                updateVisibility = value;
                OnPropertyChanged(nameof(UpdateVisibility));
            }
        }

        protected Visibility addVisibility = Visibility.Visible;
        public virtual Visibility AddVisibility
        {
            get => addVisibility;
            set
            {
                addVisibility = value;
                OnPropertyChanged(nameof(AddVisibility));
            }
        }
    }
}
