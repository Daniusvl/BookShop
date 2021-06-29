using BookShop.CRM.Core.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.CRM.Core.Base
{
    public abstract class RequestSender
    {
        protected HttpClient client;
        protected ITokenManager tokenManager;

        protected virtual async Task<TResponseModel> Send<TResponseModel, TContent>(HttpMethod method, string uri, TContent content = default)
        {
            HttpRequestMessage message = tokenManager.GenerateRequestWithToken();
            message.Method = method;
            message.RequestUri = new Uri(uri);
            message.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.SendAsync(message);
            string json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new ApiException(json);
            return JsonConvert.DeserializeObject<TResponseModel>(json);
        }
    }
}
