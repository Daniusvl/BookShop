using BookShop.CRM.Core.Base;
using BookShop.CRM.Models;
using BookShop.CRM.ViewModels.Base;
using BookShop.CRM.Wrappers;
using System.Linq;
using System.Windows;

namespace BookShop.CRM.ViewModels
{
    public class AuthorViewModel : BaseModelViewModel<AuthorModel, AuthorWrapper>
    {
        private readonly IUnitOfWork unitOfWork;

        public AuthorViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            Collection = new() { new() { Name = "New" } };
            SelectedItem = new(Collection[0]);
            LoadCommand = new Command(param => true, ExecuteLoad);
            AddCommand = new Command(CanAdd, ExecuteAdd);
            UpdateCommand = new Command(param => true, ExecuteUpdate);
            DeleteCommand = new Command(param => true, ExecuteDelete);
        }

        public override AuthorWrapper SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                if (selectedItem?.Model?.Id == 0)
                {
                    AddVisibility = Visibility.Visible;
                    UpdateVisibility = Visibility.Hidden;
                    DeleteVisibility = Visibility.Hidden;
                }
                else
                {
                    AddVisibility = Visibility.Hidden;
                    UpdateVisibility = Visibility.Visible;
                    DeleteVisibility = Visibility.Visible;
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private async void ExecuteDelete(object param)
        {
            DeleteEnabled = false;

            await SafeRequestSend(async () =>
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this item {SelectedItem.Name}",
                        "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    AuthorModel m = Collection.FirstOrDefault(a => a.Id == SelectedItem.Id);
                    Collection.Remove(m);
                    await unitOfWork.AuthorRepository.Remove(SelectedItem.Id);
                    Collection = new(Collection);
                    SelectedItem = new(Collection[0]);
                }
            });

            DeleteEnabled = true;
        }

        private async void ExecuteUpdate(object param)
        {
            UpdateEnabled = false;


            await SafeRequestSend(async () =>
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to update this item {SelectedItem.Name}",
                    "Update?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    AuthorModel model = await unitOfWork.AuthorRepository.Update(SelectedItem.Model);
                    AuthorModel previous = Collection.FirstOrDefault(a => a.Id == SelectedItem.Id);
                    int index = Collection.IndexOf(previous);
                    Collection[index] = model;
                    Collection = new(Collection);
                    SelectedItem = new(Collection[index]);
                }
            });

            UpdateEnabled = true;
        }

        private bool CanAdd(object param)
        {
            return !SelectedItem.HasErrors;
        }

        private async void ExecuteAdd(object param)
        {
            AddEnabled = false;

            await SafeRequestSend(async () =>
            {
                AuthorModel model = await unitOfWork.AuthorRepository.Add(SelectedItem.Model);
                Collection[0] = model;
                Collection.Insert(0, new AuthorModel() { Name = "New" });
                Collection = new(Collection);
                SelectedItem = new(Collection[1]);
            });

            AddEnabled = true;
        }

        private async void ExecuteLoad(object param)
        {
            LoadEnabled = false;

            await SafeRequestSend(async () =>
            {
                int selected_id = SelectedItem.Id;
                Collection = (await unitOfWork.AuthorRepository.GetAll()).ToList();
                Collection.Reverse();
                Collection.Insert(0, new AuthorModel() { Name = "New" });
                Collection = new(Collection);
                SelectedItem = new(Collection.FirstOrDefault(a => a.Id == selected_id));
            });

            LoadEnabled = true;
        }
    }
}
