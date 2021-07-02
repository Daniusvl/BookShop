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
using System.Windows;

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
            return await base.Send<PhotoModel, AddPhotoCommand>(HttpMethod.Post, Path, command);
        }

        public async Task<IList<PhotoModel>> GetAll()
        {
            return await base.Send<IList<PhotoModel>, object>(HttpMethod.Get, Path);
        }

        public async Task<PhotoModel> GetById(int id)
        {
            return await base.Send<PhotoModel, object>(HttpMethod.Get, Path + id);
        }

        public async Task Remove(int id)
        {
            await base.Send<object, object>(HttpMethod.Delete, Path + id);
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
