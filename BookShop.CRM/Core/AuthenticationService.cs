using BookShop.CRM.Core.Base;
using BookShop.CRM.Core.Exceptions;
using BookShop.CRM.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.CRM.Core
{
    public record AuthenticateCommand(string Email, string Password);

    public record Response(string ExceptionName, string Message);

    public record RefreshTokenResponse(AuthenticatedUser Auth);

    public record RefreshTokenRequest(string UserId, string RefreshToken);

    public class AuthenticationService : RequestSender, IAuthenticationService
    {
        protected const string AuthPath = "api/auth/authenticate/";
        protected const string RefreshPath = "api/auth/refreshtoken/";
        protected const string ValidatePath = "api/auth/validatetoken/";

        public AuthenticationService(IUserManager userManager)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            this.client = new(handler);
            this.client.BaseAddress = new Uri(Constants.BaseAddress);
            this.userManager = userManager;
        }

        public async Task<AuthenticatedUser> Authenticate(AuthenticateCommand command)
        {
            AuthenticatedUser user = await Send<AuthenticatedUser, AuthenticateCommand>(HttpMethod.Post, AuthPath, command);
            userManager.User = user ?? new();
            return user;
        }

        public async Task<bool> RefreshToken()
        {
            AuthenticatedUser user = (await Send<RefreshTokenResponse, RefreshTokenRequest>(HttpMethod.Post, RefreshPath,
                new(userManager.User.UserId, userManager.User.RefreshToken)))?.Auth;
            if(user != null)
            {
                userManager.User = user;
                return true;
            }
            return false;
        }

        public async Task<bool> TryAuthenticate()
        {
            try
            {
                await Send<Dictionary<string, string>, Dictionary<string, string>>(HttpMethod.Post, ValidatePath, null);
                return true;
            }
            catch (Exception ex)
            {
                if(ex.Message == "Unauthorized")
                {
                    return await RefreshToken();
                }
            }
            return false;
        }

        protected override async Task<TResponseModel> Send<TResponseModel, TContent>(HttpMethod method, string uri, TContent content = default)
        {
            HttpRequestMessage message = userManager.GenerateRequestWithToken(method, uri);
            if (!(content == null))
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            }
            HttpResponseMessage response = await client.SendAsync(message);
            string json = await response.Content.ReadAsStringAsync();
            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Unauthorized");
            }
            if (!response.IsSuccessStatusCode)
            {
                Response error_response = JsonConvert.DeserializeObject<Response>(json);
                return default;
            }
                
            return JsonConvert.DeserializeObject<TResponseModel>(json);
        }
    }
}
