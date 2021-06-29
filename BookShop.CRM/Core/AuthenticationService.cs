using BookShop.CRM.Core.Base;
using BookShop.CRM.Core.Exceptions;
using BookShop.CRM.Core.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.CRM.Core
{
    public record AuthenticateCommand(string Email, string Password);

    public record Response(string ExceptionName, string Message);

    public class AuthenticationService : RequestSender, IAuthenticationService
    {
        protected const string Path = "api/auth/authenticate/";

        public AuthenticationService(HttpClient client, IUserManager userManager)
        {
            this.client = client;
            this.userManager = userManager;
        }

        public async Task<AuthenticatedUser> Authenticate(AuthenticateCommand command)
        {
            AuthenticatedUser user = await Send<AuthenticatedUser, AuthenticateCommand>(HttpMethod.Post, Path, command);
            userManager.User = user ?? new();
            return user;
        }

        protected override async Task<TResponseModel> Send<TResponseModel, TContent>(HttpMethod method, string uri, TContent content = default)
        {
            HttpRequestMessage message = userManager.GenerateRequestWithToken();
            message.Method = method;
            message.RequestUri = new Uri(uri);
            message.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.SendAsync(message);
            string json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                Response error_response = JsonConvert.DeserializeObject<Response>(json);
                if (error_response?.ExceptionName == "UnknownException")
                    return default;
                else throw new ApiException(error_response?.Message);
            }
                
            return JsonConvert.DeserializeObject<TResponseModel>(json);
        }
    }
}
