using BookShop.CRM.Core.Base;
using BookShop.CRM.Models;
using BookShop.CRM.ViewModels.Base;
using BookShop.CRM.Wrappers;
using System.Linq;
using System.Windows;

namespace BookShop.CRM.ViewModels
{
    public class CategoryViewModel : BaseModelViewModel<CategoryModel, CategoryWrapper>
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            Collection = new() { new CategoryModel { Name = "New" } };
            SelectedItem = new(Collection[0]);

            LoadCommand = new Command(param => true, ExecuteLoad);
            AddCommand = new Command(CanAdd, ExecuteAdd);
            UpdateCommand = new Command(param => true, ExecuteUpdate);
            DeleteCommand = new Command(param => true, ExecuteDelete);
        }

        public override CategoryWrapper SelectedItem
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

        public async void ExecuteLoad(object param)
        {
            LoadEnabled = false;

            await SafeRequestSend(async () =>
            {
                int id = selectedItem.Id;
                Collection = (await unitOfWork.CategoryRepository.GetAll()).ToList();
                Collection.Reverse();
                Collection.Insert(0, new CategoryModel { Name = "New" });
                Collection = new(Collection);
                SelectedItem = new(Collection.FirstOrDefault(c => c.Id == id));
            });

            LoadEnabled = true;
        }

        public bool CanAdd(object param)
        {
            return !SelectedItem.HasErrors;
        }

        public async void ExecuteAdd(object param)
        {
            AddEnabled = false;

            await SafeRequestSend(async () =>
            {
                CategoryModel category = await unitOfWork.CategoryRepository.Add(SelectedItem.Model);
                Collection[0] = category;
                Collection.Insert(0, new CategoryModel { Name = "New" });
                Collection = new(Collection);
                SelectedItem = new(Collection[1]);
            });

            AddEnabled = true;
        }

        public async void ExecuteUpdate(object param)
        {
            UpdateEnabled = false;

            await SafeRequestSend(async () =>
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to update this item {SelectedItem.Name}",
                        "Update?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    CategoryModel category = await unitOfWork.CategoryRepository.Update(SelectedItem.Model);
                    CategoryModel previous = Collection.FirstOrDefault(c => c.Id == SelectedItem.Id);
                    int index = Collection.IndexOf(previous);
                    Collection[index] = category;
                    Collection = new(Collection);
                    SelectedItem = new(Collection[index]);
                }
            });

            UpdateEnabled = true;
        }

        public async void ExecuteDelete(object param)
        {
            DeleteEnabled = false;

            await SafeRequestSend(async () =>
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this item {SelectedItem.Name}",
                    "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await unitOfWork.CategoryRepository.Remove(SelectedItem.Id);
                    CategoryModel previous = Collection.FirstOrDefault(c => c.Id == SelectedItem.Id);
                    Collection.Remove(previous);
                    Collection = new(Collection);
                    SelectedItem = new(Collection[0]);
                }
            });

            DeleteEnabled = true;
        }
    }
}
