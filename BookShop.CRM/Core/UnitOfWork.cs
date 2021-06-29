using BookShop.CRM.Core.Base;
using System;
using System.Net.Http;

namespace BookShop.CRM.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public const string BasePath = "https://localhost:4004";

        
        private readonly IUserManager userManager;
        
        private HttpClient Client;

        private IBookRepository bookRepository;

        private ICategoryRepository categoryRepository;

        private IAuthorRepository authorRepository;

        private IPhotoRepository photoRepository;

        public UnitOfWork(IUserManager userManager)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new(handler);
            Client.BaseAddress = new Uri(BasePath);
            this.userManager = userManager;
        }

        public IBookRepository BookRepository => bookRepository ??= new BookRepository(Client, userManager);

        public ICategoryRepository CategoryRepository => categoryRepository ??= new CategoryRepository(Client, userManager);

        public IAuthorRepository AuthorRepository => authorRepository ??= new AuthorRepository(Client, userManager);

        public IPhotoRepository PhotoRepository => photoRepository ??= new PhotoRepository(Client, userManager);
    }
}
