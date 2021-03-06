using BookShop.CRM.Core.Base;
using BookShop.CRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookShop.CRM.Core
{
    public record AddCategoryCommand(string Name);

    public record UpdateCategoryCommand(int Id, string Name);

    public class CategoryRepository : RequestSender, ICategoryRepository
    {
        protected const string Path = "api/categories/";

        public CategoryRepository(HttpClient client, IUserManager userManager, IAuthenticationService authenticationService)
        {
            this.client = client;
            this.userManager = userManager;
            this.authenticationService = authenticationService;
        }

        public async Task<CategoryModel> Add(AddCategoryCommand command)
        {
            return await Send<CategoryModel, AddCategoryCommand>(HttpMethod.Post, Path, command);
        }

        public async Task<IList<CategoryModel>> GetAll()
        {
            return await Send<IList<CategoryModel>, object>(HttpMethod.Get, Path);
        }

        public async Task<CategoryModel> GetById(int id)
        {
            return await Send<CategoryModel, object>(HttpMethod.Get, Path + id);
        }

        public async Task Remove(int id)
        {
             await Send<object, object>(HttpMethod.Delete, Path + id);
        }

        public async Task<CategoryModel> Update(UpdateCategoryCommand command)
        {
            return await Send<CategoryModel, UpdateCategoryCommand>(HttpMethod.Put, Path, command);
        }
    }
}
