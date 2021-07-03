using BookShop.CRM.Core;
using BookShop.CRM.Core.Base;
using BookShop.CRM.Core.Exceptions;
using BookShop.CRM.Core.Models;
using BookShop.CRM.Models;
using BookShop.CRM.ViewModels.Base;
using BookShop.CRM.Wrappers;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        private readonly IUnitOfWork unitOfWork;

        public BookViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            Books = new();
            Books.Add(new() { Name = "New" });
            SelectedBook = new(Books[0]);
            LoadCommand = new Command(param => true, ExecuteLoad);
            AddCommand = new Command(CanAdd, ExecuteAdd);
            AddFile = new Command(CanAddFile, ExecuteAddFile);
            UpdateCommand = new Command(param => true, ExecuteUpdate);
            DeleteCommand = new Command(param => true, ExecuteDelete);
        }

        private List<BookModel> books;
        public List<BookModel> Books 
        { 
            get => books;
            set
            {
                books = value;
                OnPropertyChanged(nameof(Books));
            } 
        }

        private Visibility deleteBookVisibility = Visibility.Visible;
        public Visibility DeleteBookVisibility
        {
            get => deleteBookVisibility;
            set
            {
                deleteBookVisibility = value;
                OnPropertyChanged(nameof(DeleteBookVisibility));
            }
        }

        private Visibility updateBookVisibility = Visibility.Visible;
        public Visibility UpdateBookVisibility
        {
            get => updateBookVisibility;
            set
            {
                updateBookVisibility = value;
                OnPropertyChanged(nameof(UpdateBookVisibility));
            }
        }

        private Visibility addBookVisibility = Visibility.Visible;
        public Visibility AddBookVisibility
        {
            get => addBookVisibility;
            set
            {
                addBookVisibility = value;
                OnPropertyChanged(nameof(AddBookVisibility));
            }
        }

        private Visibility bookAddFileSectionVisibility = Visibility.Visible;
        public Visibility BookAddFileSectionVisibility
        {
            get => bookAddFileSectionVisibility;
            set
            {
                bookAddFileSectionVisibility = value;
                OnPropertyChanged(nameof(BookAddFileSectionVisibility));
            }
        }


        public void ExecuteDelete(object param)
        {
            DeleteBookEnabled = false;
            if (selectedBook.Id == 0)
            {
                MessageBox.Show("This item does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DeleteBookEnabled = true;
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this item {SelectedBook.Name}",
                "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                try
                {
                    unitOfWork.BookRepository.Remove(selectedBook.Id);
                    List<BookModel> new_books = Books;
                    BookModel book = new_books.SingleOrDefault(b => b.Id == selectedBook.Id);
                    new_books.Remove(book);
                    Books = new(new_books);
                    SelectedBook = new(Books[0]);
                }
                catch (ApiException e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            DeleteBookEnabled = true;
        }
        public ICommand DeleteCommand { get; }


        public async void ExecuteUpdate(object param)
        {
            UpdateBookEnabled = false;

            if (selectedBook.Id == 0)
            {
                MessageBox.Show("This item does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateBookEnabled = true;
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to update this item {SelectedBook.Name}",
                "Update?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    BookModel book = await unitOfWork.BookRepository.Update(selectedBook.Model);
                    List<BookModel> new_books = Books;
                    BookModel outdated = new_books.FirstOrDefault(b => b.Id == book.Id);
                    int index = new_books.IndexOf(outdated);
                    new_books.Remove(outdated);
                    new_books.Insert(index, book);
                    Books = new(new_books);
                    SelectedBook = new(Books[index]);
                }
                catch (ApiException e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

            UpdateBookEnabled = true;
        }

        public ICommand UpdateCommand { get; }

        public bool CanAddFile(object param)
        {
            return SelectedBook.Model.Id == 0;
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

        public ICommand AddFile { get; }

        public bool CanAdd(object param)
        {
            return !SelectedBook.HasErrors;
        }

        public async void ExecuteAdd(object param)
        {
            AddBookEnabled = false;
            if (string.IsNullOrEmpty(FilePath))
            {
                MessageBox.Show("You need to add file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AddBookEnabled = true;
                return;
            }

            AddBookCommand command = SelectedBook.Model;
            try
            {
                BookModel book = await unitOfWork.BookRepository.Add(command);
                if (book.Id != 0)
                {
                    await unitOfWork.BookRepository.UploadFile(FilePath, book.Id);
                    List<BookModel> new_books = Books;
                    new_books[0] = book;
                    new_books.Insert(0, new() { Name = "New" });
                    Books = new(new_books);
                    SelectedBook = new(new_books[1]);
                    FilePath = string.Empty;
                }
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            AddBookEnabled = true;
        }
        public ICommand AddCommand { get; }

        private async void ExecuteLoad(object param)
        {
            LoadBooksEnabled = false;
            int selected_id = SelectedBook.Model.Id;
            try
            {
                List<BookModel> books = (await unitOfWork.BookRepository.GetAll()).ToList();
                books.Reverse();
                books.Insert(0, new() { Name = "New" });
                Books = books;
                SelectedBook = new(Books.FirstOrDefault(b => b.Id == selected_id));
            }
            catch (ApiException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadBooksEnabled = true;
        }

        public ICommand LoadCommand { get; }

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

        private BookWrapper selectedBook;
        public BookWrapper SelectedBook 
        {
            get => selectedBook;
            set 
            {
                selectedBook = value;
                if (selectedBook?.Model?.Id == 0)
                {
                    BookAddFileSectionVisibility = Visibility.Visible;
                    AddBookVisibility = Visibility.Visible;
                    UpdateBookVisibility = Visibility.Hidden;
                    DeleteBookVisibility = Visibility.Hidden;
                }
                else 
                {
                    BookAddFileSectionVisibility = Visibility.Hidden;
                    AddBookVisibility = Visibility.Hidden;
                    UpdateBookVisibility = Visibility.Visible;
                    DeleteBookVisibility = Visibility.Visible;
                }
                OnPropertyChanged(nameof(SelectedBook));
            } 
        }

        private bool deleteBookEnabled = true;
        public bool DeleteBookEnabled
        {
            get => deleteBookEnabled;
            set
            {
                deleteBookEnabled = value;
                OnPropertyChanged(nameof(DeleteBookEnabled));
            }
        }

        private bool updateBookEnabled = true;
        public bool UpdateBookEnabled
        {
            get => updateBookEnabled;
            set
            {
                updateBookEnabled = value;
                OnPropertyChanged(nameof(UpdateBookEnabled));
            }
        }

        private bool addBookEnabled = true;
        public bool AddBookEnabled
        {
            get => addBookEnabled;
            set
            {
                addBookEnabled = value;
                OnPropertyChanged(nameof(AddBookEnabled));
            }
        }

        private bool loadBooksEnabled = true;
        public bool LoadBooksEnabled
        {
            get => loadBooksEnabled;
            set
            {
                loadBooksEnabled = value;
                OnPropertyChanged(nameof(LoadBooksEnabled));
            }
        }
    }
}
