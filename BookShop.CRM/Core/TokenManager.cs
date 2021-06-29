using BookShop.CRM.Core.Base;
using BookShop.CRM.Core.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BookShop.CRM.Core
{
    public class TokenManager : ITokenManager
    {
        public const string Path = "token.txt";

        private TokenModel token;
        public TokenModel Token
        {
            get => token ??= new(); 
            set
            {
                Token = value;
                Write();
            }
        }

        public TokenManager()
        {
            Read();
        }

        public HttpRequestMessage GenerateRequestWithToken()
        {
            HttpRequestMessage message = new();
            if (!string.IsNullOrEmpty(Token.Token))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.Token);
            }
            return message;
        }

        protected virtual void Write()
        {
            string json = JsonConvert.SerializeObject(Token);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            File.WriteAllBytes(Path, bytes);
        }

        protected virtual TokenModel Read()
        {
            if (!File.Exists(Path))
                return Token;
            byte[] bytes = File.ReadAllBytes(Path);
            string json = Encoding.UTF8.GetString(bytes);
            Token = JsonConvert.DeserializeObject<TokenModel>(json);
            return Token;
        }
    }
}
