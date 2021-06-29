using BookShop.CRM.Core.Models;
using System.Net.Http;

namespace BookShop.CRM.Core.Base
{
    public interface ITokenManager
    {
        TokenModel Token { get; set; }

        HttpRequestMessage GenerateRequestWithToken();
    }
}