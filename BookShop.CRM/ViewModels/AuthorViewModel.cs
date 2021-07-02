using BookShop.CRM.Core;
using BookShop.CRM.Core.Base;
using BookShop.CRM.Core.Exceptions;
using BookShop.CRM.Models;
using BookShop.CRM.ViewModels.Base;
using BookShop.CRM.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels
{
    public class AuthorViewModel : BaseViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        public AuthorViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            List<AuthorModel> authors = new() { new() { Name = "New" } };
            Authors = authors;
            SelectedAuthor = new(Authors[0]);
            LoadCommand = new Command(param => true, ExecuteLoad);
            AddCommand = new Command(CanAdd, ExecuteAdd);
            UpdateCommand = new Command(param => true, ExecuteUpdate);
            DeleteCommand = new Command(param => true, ExecuteDelete);
        }

        private async void ExecuteDelete(object param)
        {
            DeleteEnabled = false;

            try
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this item {SelectedAuthor.Name}",
                "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    List<AuthorModel> authors = Authors;
                    AuthorModel m = authors.FirstOrDefault(a => a.Id == selectedAuthor.Model.Id);
                    authors.Remove(m);
                    await unitOfWork.AuthorRepository.Remove(selectedAuthor.Model.Id);
                    Authors = new(authors);
                    SelectedAuthor = new(Authors[0]);
                }
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DeleteEnabled = true;
        }

        public ICommand DeleteCommand { get; }

        private async void ExecuteUpdate(object param)
        {
            UpdateEnabled = false;

            try
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to update this item {SelectedAuthor.Name}",
                "Update?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                { 
                    AuthorModel model = await unitOfWork.AuthorRepository.Update(selectedAuthor.Model);
                    List<AuthorModel> authors = Authors;
                    AuthorModel previous = authors.FirstOrDefault(a => a.Id == selectedAuthor.Model.Id);
                    int index = authors.IndexOf(previous);
                    authors[index] = model;
                    Authors = new(authors);
                    SelectedAuthor = new(Authors[index]);
                }
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UpdateEnabled = true;
        }

        public ICommand UpdateCommand { get; }

        private bool CanAdd(object param)
        {
            return !SelectedAuthor.HasErrors;
        }

        private async void ExecuteAdd(object param)
        {
            AddEnabled = false;
            try
            {
                AddAuthorCommand command = selectedAuthor.Model;
                AuthorModel model = await unitOfWork.AuthorRepository.Add(command);
                List<AuthorModel> authors = Authors;
                authors[0] = model;
                authors.Insert(0, new AuthorModel() { Name = "New" });
                Authors = new(authors);
                SelectedAuthor = new(Authors[1]);

            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            AddEnabled = true;
        }

        public ICommand AddCommand { get; }

        private async void ExecuteLoad(object param)
        {
            LoadEnabled = false;
            try
            {
                int selected_id = SelectedAuthor.Model.Id;
                List<AuthorModel> authors = (await unitOfWork.AuthorRepository.GetAll()).ToList();
                authors.Reverse();
                authors.Insert(0, new AuthorModel() { Name = "New" });
                Authors = new(authors);
                SelectedAuthor = new(authors.FirstOrDefault(a => a.Id == selected_id));
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LoadEnabled = true;
        }

        public ICommand LoadCommand { get; }

        private List<AuthorModel> authors;
        public List<AuthorModel> Authors
        {
            get => authors;
            set
            {
                authors = value;
                OnPropertyChanged(nameof(Authors));
            }
        }

        private AuthorWrapper selectedAuthor;
        public AuthorWrapper SelectedAuthor
        {
            get => selectedAuthor;
            set
            {
                selectedAuthor = value;
                if(selectedAuthor?.Model?.Id == 0)
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
                OnPropertyChanged(nameof(SelectedAuthor));
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
    }
}
