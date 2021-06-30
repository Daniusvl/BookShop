using BookShop.CRM.Core.Base;
using BookShop.CRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookShop.CRM.Core
{
    public record AddAuthorCommand(string Name);

    public record UpdateAuthorCommand(int Id, string Name);

    public class AuthorRepository : RequestSender, IAuthorRepository
    {
        protected const string Path = "api/authors/";

        public AuthorRepository(HttpClient client, IUserManager userManager, IAuthenticationService authenticationService)
        {
            this.client = client;
            this.userManager = userManager;
            this.authenticationService = authenticationService;
        }

        public async Task<AuthorModel> Add(AddAuthorCommand command)
        {
            return await Send<AuthorModel, AddAuthorCommand>(HttpMethod.Post, Path, command);
        }

        public async Task<IList<AuthorModel>> GetAll()
        {
            return await Send<IList<AuthorModel>, object>(HttpMethod.Get, Path);
        }

        public async Task<AuthorModel> GetById(int id)
        {
            return await Send<AuthorModel, object>(HttpMethod.Get, Path + id);
        }

        public async Task Remove(int id)
        {
            await Send<object, object>(HttpMethod.Delete, Path + id);
        }

        public async Task<AuthorModel> Update(UpdateAuthorCommand command)
        {
            return await Send<AuthorModel, UpdateAuthorCommand>(HttpMethod.Put, Path, command);
        }
    }
}
