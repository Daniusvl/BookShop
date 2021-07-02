using BookShop.CRM.Core.Base;
using BookShop.CRM.ViewModels.Base;
using System;
using System.Windows;
using System.Windows.Input;

namespace BookShop.CRM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IUserManager userManager;

        public MainViewModel(IUserManager userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            UserName = $"Hello, {this.userManager.User.UserName}!";
            LogoutCommand = new Command( param => true, ExecuteLogout);

            BookPressed = false;
            AuthorPressed = false;
            CategoryPressed = false;
            PhotoPressed = false;

            VisibilityHideAll();

            book = new(unitOfWork);
            author = new(unitOfWork);
            category = new(unitOfWork);
        }

        private BookViewModel book;
        public BookViewModel Book 
        { 
            get => book;
            set
            {
                book = value;
                OnPropertyChanged(nameof(Book));
            } 
        }

        private AuthorViewModel author;
        public AuthorViewModel Author
        {
            get => author;
            set
            {
                author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private CategoryViewModel category;
        public CategoryViewModel Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        #region Visibilities

        private Visibility booksVisibility;
        public Visibility BooksVisibility 
        {
            get => booksVisibility;
            set
            {
                booksVisibility = value;
                OnPropertyChanged(nameof(BooksVisibility));
            }
        }

        private Visibility authorsVisibility;
        public Visibility AuthorsVisibility
        {
            get => authorsVisibility;
            set
            {
                authorsVisibility = value;
                OnPropertyChanged(nameof(AuthorsVisibility));
            }
        }

        private Visibility categoriesVisibility;
        public Visibility CategoriesVisibility
        {
            get => categoriesVisibility;
            set
            {
                categoriesVisibility = value;
                OnPropertyChanged(nameof(CategoriesVisibility));
            }
        }

        private Visibility photosVisibility;
        public Visibility PhotosVisibility
        {
            get => photosVisibility;
            set
            {
                photosVisibility = value;
                OnPropertyChanged(nameof(PhotosVisibility));
            }
        }

        #endregion

        #region RadioButtons
        
        private bool bookPressed;
        public bool BookPressed 
        {
            get => bookPressed;
            set
            {
                bookPressed = value;
                if (value)
                {
                    SetVisibility(nameof(BooksVisibility));
                }
                OnPropertyChanged(nameof(BookPressed));
            }
        }

        private bool authorPressed;
        public bool AuthorPressed
        {
            get => authorPressed;
            set
            {
                authorPressed = value;
                if (value)
                {
                    SetVisibility(nameof(AuthorsVisibility));
                }
                OnPropertyChanged(nameof(AuthorPressed));
            }
        }

        private bool categoryPressed;
        public bool CategoryPressed
        {
            get => categoryPressed;
            set
            {
                categoryPressed = value;
                if (value)
                {
                    SetVisibility(nameof(CategoriesVisibility));
                }
                OnPropertyChanged(nameof(CategoryPressed));
            }
        }

        private bool photoPressed;
        public bool PhotoPressed
        {
            get => photoPressed;
            set
            {
                photoPressed = value;
                if (value)
                {
                    SetVisibility(nameof(PhotosVisibility));
                }
                OnPropertyChanged(nameof(PhotoPressed));
            }
        }

        #endregion

        private void SetVisibility(string visibilityParamName)
        {
            switch (visibilityParamName)
            {
                case "BooksVisibility":
                    BooksVisibility = Visibility.Visible;
                    AuthorsVisibility = Visibility.Hidden;
                    CategoriesVisibility = Visibility.Hidden;
                    PhotosVisibility = Visibility.Hidden;
                    break;
                case "AuthorsVisibility":
                    BooksVisibility = Visibility.Hidden;
                    AuthorsVisibility = Visibility.Visible;
                    CategoriesVisibility = Visibility.Hidden;
                    PhotosVisibility = Visibility.Hidden;
                    break;
                case "CategoriesVisibility":
                    BooksVisibility = Visibility.Hidden;
                    AuthorsVisibility = Visibility.Hidden;
                    CategoriesVisibility = Visibility.Visible;
                    PhotosVisibility = Visibility.Hidden;
                    break;
                case "PhotosVisibility":
                    BooksVisibility = Visibility.Hidden;
                    AuthorsVisibility = Visibility.Hidden;
                    CategoriesVisibility = Visibility.Hidden;
                    PhotosVisibility = Visibility.Visible;
                    break;
            }
        }

        private void VisibilityHideAll()
        {
            BooksVisibility = Visibility.Hidden;
            AuthorsVisibility = Visibility.Hidden;
            CategoriesVisibility = Visibility.Hidden;
            PhotosVisibility = Visibility.Hidden;
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
