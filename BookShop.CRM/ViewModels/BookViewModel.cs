using BookShop.CRM.Core.Base;
using BookShop.CRM.Models;
using BookShop.CRM.ViewModels.Base;
using BookShop.CRM.Wrappers;
using Microsoft.Win32;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels
{
    public class BookViewModel : BaseModelViewModel<BookModel, BookWrapper>
    {
        private readonly IUnitOfWork unitOfWork;

        public BookViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            Collection = new();
            Collection.Add(new() { Name = "New" });
            SelectedItem = new(Collection[0]);
            LoadCommand = new Command(param => true, ExecuteLoad);
            AddCommand = new Command(CanAdd, ExecuteAdd);
            AddFileCommand = new Command(CanAddFile, ExecuteAddFile);
            UpdateCommand = new Command(param => true, ExecuteUpdate);
            DeleteCommand = new Command(param => true, ExecuteDelete);
        }

        public override BookWrapper SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                if (selectedItem?.Model?.Id == 0)
                {
                    AddFileSectionVisibility = Visibility.Visible;
                    AddVisibility = Visibility.Visible;
                    UpdateVisibility = Visibility.Hidden;
                    DeleteVisibility = Visibility.Hidden;
                }
                else
                {
                    AddFileSectionVisibility = Visibility.Hidden;
                    AddVisibility = Visibility.Hidden;
                    UpdateVisibility = Visibility.Visible;
                    DeleteVisibility = Visibility.Visible;
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private Visibility addFileSectionVisibility = Visibility.Visible;
        public Visibility AddFileSectionVisibility
        {
            get => addFileSectionVisibility;
            set
            {
                addFileSectionVisibility = value;
                OnPropertyChanged(nameof(AddFileSectionVisibility));
            }
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
                    await unitOfWork.BookRepository.Remove(SelectedItem.Id);
                    BookModel book = Collection.SingleOrDefault(b => b.Id == SelectedItem.Id);
                    Collection.Remove(book);
                    Collection = new(Collection);
                    SelectedItem = new(Collection[0]);
                }
            });
            
            DeleteEnabled = true;
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
                    BookModel book = await unitOfWork.BookRepository.Update(SelectedItem.Model);
                    BookModel outdated = Collection.FirstOrDefault(b => b.Id == book.Id);
                    int index = Collection.IndexOf(outdated);
                    Collection.Remove(outdated);
                    Collection.Insert(index, book);
                    Collection = new(Collection);
                    SelectedItem = new(Collection[index]);
                }
            });

            UpdateEnabled = true;
        }

        public bool CanAddFile(object param)
        {
            return SelectedItem.Id == 0;
        }

        public void ExecuteAddFile(object param)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pdf Files|*.pdf";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        public ICommand AddFileCommand { get; }

        public bool CanAdd(object param)
        {
            return !SelectedItem.HasErrors;
        }

        public async void ExecuteAdd(object param)
        {
            AddEnabled = false;

            await SafeRequestSend(async () =>
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    MessageBox.Show("You need to add file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    AddEnabled = true;
                    return;
                }
                BookModel book = await unitOfWork.BookRepository.Add(SelectedItem.Model);
                if (book.Id != 0)
                {
                    await unitOfWork.BookRepository.UploadFile(FilePath, book.Id);
                    Collection[0] = book;
                    Collection.Insert(0, new() { Name = "New" });
                    Collection = new(Collection);
                    SelectedItem = new(Collection[1]);
                    FilePath = string.Empty;
                }
            });

            AddEnabled = true;
        }

        private async void ExecuteLoad(object param)
        {
            LoadEnabled = false;
            await SafeRequestSend(async () =>
            {
                int selected_id = SelectedItem.Id;
                Collection = (await unitOfWork.BookRepository.GetAll()).ToList();
                Collection.Reverse();
                Collection.Insert(0, new() { Name = "New" });
                Collection = new(Collection);
                SelectedItem = new(Collection.FirstOrDefault(b => b.Id == selected_id));
            });
            LoadEnabled = true;
        }

        private string filePath = string.Empty;
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }
    }
}
