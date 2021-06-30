using BookShop.CRM.Core.Base;
using BookShop.CRM.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookShop.CRM.Core
{
    public record AddBookCommand(string Name, string Description, decimal Price, bool hidden,
        DateTime DateReleased, int AuthorId, int CategoryId, IList<byte> bytes);

    public record UpdateBookCommand(int Id, string Name, string Description, decimal Price, bool hidden,
        DateTime DateReleased, int AuthorId, int CategoryId, IList<byte> bytes);

    public class BookRepository : RequestSender, IBookRepository
    {
        protected const string Path = "api/books/";

        public BookRepository(HttpClient client, IUserManager userManager, IAuthenticationService authenticationService)
        {
            this.client = client;
            this.userManager = userManager;
            this.authenticationService = authenticationService;
        }

        public async Task<BookModel> Add(AddBookCommand command)
        {
            return await Send<BookModel, AddBookCommand>(HttpMethod.Post, Path, command);
        }

        public async Task<IList<BookModel>> GetAll()
        {
            return await Send<IList<BookModel>, object>(HttpMethod.Get, Path);
        }

        public async Task<BookModel> GetById(int id)
        {
            return await Send<BookModel, object>(HttpMethod.Get, Path + id);
        }

        public async Task Remove(int id)
        {
            await Send<object, object>(HttpMethod.Delete, Path + id);
        }

        public async Task<BookModel> Update(UpdateBookCommand command)
        {
            return await Send<BookModel, UpdateBookCommand>(HttpMethod.Put, Path, command);
        }
    }
}
