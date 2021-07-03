using BookShop.CRM.Core.Base;
using BookShop.CRM.Core.Exceptions;
using BookShop.CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookShop.CRM.Core
{
    public record AddBookCommand(string Name, string Description, decimal Price, bool Hidden,
        DateTime DateReleased, int AuthorId, int CategoryId);

    public record UpdateBookCommand(int Id, string Name, string Description, decimal Price, bool Hidden,
                                        DateTime DateReleased, int AuthorId, int CategoryId);

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
            return await base.Send<BookModel, AddBookCommand>(HttpMethod.Post, Path, command);
        }

        public async Task<IList<BookModel>> GetAll()
        {
            return await base.Send<IList<BookModel>, object>(HttpMethod.Get, Path);
        }

        public async Task<BookModel> GetById(int id)
        {
            return await base.Send<BookModel, object>(HttpMethod.Get, Path + id);
        }

        public async Task Remove(int id)
        {
            await base.Send<object, object>(HttpMethod.Delete, Path + id);
        }

        public async Task<BookModel> Update(UpdateBookCommand command)
        {
            return await base.Send<BookModel, UpdateBookCommand>(HttpMethod.Put, Path, command);
        }

        public async Task UploadFile(string path, int id)
        {
            await Send<object, string>(HttpMethod.Post, $"{Path}UploadFile/{id}", path);
        }

        protected override async Task<TResponseModel> Send<TResponseModel, TContent>(HttpMethod method, string uri, TContent content = default)
        {
            if (content is string path)
            {
                FileStream fileStream = new(path, FileMode.OpenOrCreate);

                HttpRequestMessage message = userManager.GenerateRequestWithToken(method, uri);
                message.Content = new StreamContent(fileStream, (int)fileStream.Length);
                HttpResponseMessage response = await client.SendAsync(message);
                string json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    bool result = await authenticationService.RefreshToken();
                    if (!result)
                    {
                        throw new Exception("Unauthorized");
                    }
                    else
                    {
                        return await Send<TResponseModel, TContent>(method, uri, content);
                    }
                }
                Response res = JsonConvert.DeserializeObject<Response>(json);
                if (res?.ExceptionName == "CommonException" || res?.ExceptionName == "NotFoundException")
                {
                    throw new ApiException(res.Message);
                }
                else if (res.ExceptionName == "ValidationException")
                {
                    throw new ApiException("Invalid data provided");
                }
                else throw new ApiException("Unknown error");
            }
            return default;
        }
    }
}
