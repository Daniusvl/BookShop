using BookShop.CRM.Core.Models;
using System.Net.Http;

namespace BookShop.CRM.Core.Base
{
    public interface IUserManager
    {
        AuthenticatedUser User { get; set; }

        HttpRequestMessage GenerateRequestWithToken(HttpMethod method, string uri);
    }
}