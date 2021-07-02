using BookShop.CRM.Core;
using BookShop.CRM.Core.Base;
using BookShop.CRM.Core.Exceptions;
using BookShop.CRM.Models;
using BookShop.CRM.ViewModels.Base;
using BookShop.CRM.Wrappers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            List<CategoryModel> categories = new();
            categories.Add(new CategoryModel { Name = "New" });
            Categories = categories;
            SelectedCategory = new(Categories[0]);

            LoadCommand = new Command(param => true, ExecuteLoad);
            AddCommand = new Command(CanAdd, ExecuteAdd);
            UpdateCommand = new Command(param => true, ExecuteUpdate);
            DeleteCommand = new Command(param => true, ExecuteDelete);
        }

        public ICommand LoadCommand { get; }
        
        public ICommand AddCommand { get; }
        
        public ICommand UpdateCommand { get; }

        public ICommand DeleteCommand { get; }

        public async void ExecuteLoad(object param)
        {
            LoadEnabled = false;

            try
            {
                int id = selectedCategory.Model.Id;
                List<CategoryModel> categories = (await unitOfWork.CategoryRepository.GetAll()).ToList();
                categories.Reverse();
                categories.Insert(0, new CategoryModel { Name = "New" });
                Categories = new(categories);
                SelectedCategory = new(Categories.FirstOrDefault(c => c.Id == id));
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LoadEnabled = true;
        }

        public bool CanAdd(object param)
        {
            return !SelectedCategory.HasErrors;
        }

        public async void ExecuteAdd(object param)
        {
            AddEnabled = false;
            try
            {
                AddCategoryCommand command = selectedCategory.Model;
                CategoryModel category = await unitOfWork.CategoryRepository.Add(command);
                List<CategoryModel> categories = Categories;
                categories[0] = category;
                categories.Insert(0, new CategoryModel { Name = "New" });
                Categories = new(categories);
                SelectedCategory = new(Categories[1]);
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            AddEnabled = true;
        }

        public async void ExecuteUpdate(object param)
        {
            UpdateEnabled = false;
            try
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to update this item {SelectedCategory.Name}",
                "Update?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    UpdateCategoryCommand command = selectedCategory.Model;
                    CategoryModel category = await unitOfWork.CategoryRepository.Update(command);
                    List<CategoryModel> categories = Categories;
                    CategoryModel previous = categories.FirstOrDefault(c => c.Id == selectedCategory.Model.Id);
                    int index = categories.IndexOf(previous);
                    categories[index] = category;
                    Categories = new(categories);
                    SelectedCategory = new(Categories[index]);
                }
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateEnabled = true;
        }

        public async void ExecuteDelete(object param)
        {
            DeleteEnabled = false;
            try
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this item {SelectedCategory.Name}",
                "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    await unitOfWork.CategoryRepository.Remove(selectedCategory.Model.Id);
                    List<CategoryModel> categories = Categories;
                    CategoryModel previous = categories.FirstOrDefault(c => c.Id == selectedCategory.Model.Id);
                    categories.Remove(previous);
                    Categories = new(categories);
                    SelectedCategory = new(Categories[0]);
                }
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            DeleteEnabled = true;
        }

        private List<CategoryModel> categories;
        public List<CategoryModel> Categories 
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }        
        }

        private CategoryWrapper selectedCategory;
        public CategoryWrapper SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                if(selectedCategory?.Model?.Id == 0)
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
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private Visibility deleteVisibility = Visibility.Visible;
        public Visibility DeleteVisibility
        {
            get => deleteVisibility;
            set
            {
                deleteVisibility = value;
                OnPropertyChanged(nameof(DeleteVisibility));
            }
        }

        private Visibility updateVisibility = Visibility.Visible;
        public Visibility UpdateVisibility
        {
            get => updateVisibility;
            set
            {
                updateVisibility = value;
                OnPropertyChanged(nameof(UpdateVisibility));
            }
        }

        private Visibility addVisibility = Visibility.Visible;
        public Visibility AddVisibility
        {
            get => addVisibility;
            set
            {
                addVisibility = value;
                OnPropertyChanged(nameof(AddVisibility));
            }
        }

        private bool deleteEnabled = true;
        public bool DeleteEnabled
        {
            get => deleteEnabled;
            set
            {
                deleteEnabled = value;
                OnPropertyChanged(nameof(DeleteEnabled));
            }
        }

        private bool updateEnabled = true;
        public bool UpdateEnabled
        {
            get => updateEnabled;
            set
            {
                updateEnabled = value;
                OnPropertyChanged(nameof(UpdateEnabled));
            }
        }

        private bool addEnabled = true;
        public bool AddEnabled 
        { 
            get => addEnabled; 
            set
            {
                addEnabled = value;
                OnPropertyChanged(nameof(AddEnabled));
            }
        }

        private bool loadEnabled = true;
        public bool LoadEnabled
        {
            get => loadEnabled;
            set
            {
                loadEnabled = value;
                OnPropertyChanged(nameof(LoadEnabled));
            }
        }
    }
}
