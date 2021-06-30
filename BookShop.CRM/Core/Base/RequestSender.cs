using BookShop.CRM.Core.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.CRM.Core.Base
{
    public abstract class RequestSender
    {
        protected HttpClient client;
        protected IUserManager userManager;
        protected IAuthenticationService authenticationService;

        protected virtual async Task<TResponseModel> Send<TResponseModel, TContent>(HttpMethod method, string uri, TContent content = default)
        {
            HttpRequestMessage message = userManager.GenerateRequestWithToken(method, uri);
            message.Method = method;
            message.RequestUri = new Uri(uri);
            message.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.SendAsync(message);
            string json = await response.Content.ReadAsStringAsync();
            if(response.StatusCode == HttpStatusCode.Unauthorized)
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
            if (!response.IsSuccessStatusCode)
                throw new ApiException(json);
            return JsonConvert.DeserializeObject<TResponseModel>(json);
        }
    }
}
