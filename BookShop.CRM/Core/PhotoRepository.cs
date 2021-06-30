using BookShop.CRM.Core.Base;
using BookShop.CRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookShop.CRM.Core
{
    public record AddPhotoCommand(string ProductName, IList<byte> FileBytes);

    public class PhotoRepository : RequestSender, IPhotoRepository
    {
        protected const string Path = "api/photos/";

        public PhotoRepository(HttpClient client, IUserManager userManager, IAuthenticationService authenticationService)
        {
            this.client = client;
            this.userManager = userManager;
            this.authenticationService = authenticationService;
        }

        public async Task<PhotoModel> Add(AddPhotoCommand command)
        {
            return await Send<PhotoModel, AddPhotoCommand>(HttpMethod.Post, Path, command);
        }

        public async Task<IList<PhotoModel>> GetAll()
        {
            return await Send<IList<PhotoModel>, object>(HttpMethod.Get, Path);
        }

        public async Task<PhotoModel> GetById(int id)
        {
            return await Send<PhotoModel, object>(HttpMethod.Get, Path + id);
        }

        public async Task Remove(int id)
        {
            await Send<object, object>(HttpMethod.Delete, Path + id);
        }
    }
}
