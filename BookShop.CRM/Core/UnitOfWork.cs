using BookShop.CRM.Core.Base;
using System;
using System.Net.Http;

namespace BookShop.CRM.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public const string BasePath = "https://localhost:4004";

        
        private readonly ITokenManager tokenManager;
        
        private HttpClient Client;

        private IBookRepository bookRepository;

        private ICategoryRepository categoryRepository;

        private IAuthorRepository authorRepository;

        private IPhotoRepository photoRepository;

        public UnitOfWork(ITokenManager tokenManager)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new(handler);
            Client.BaseAddress = new Uri(BasePath);
            this.tokenManager = tokenManager;
        }

        public IBookRepository BookRepository => bookRepository ??= new BookRepository(Client, tokenManager);

        public ICategoryRepository CategoryRepository => categoryRepository ??= new CategoryRepository(Client, tokenManager);

        public IAuthorRepository AuthorRepository => authorRepository ??= new AuthorRepository(Client, tokenManager);

        public IPhotoRepository PhotoRepository => photoRepository ??= new PhotoRepository(Client, tokenManager);
    }
}
