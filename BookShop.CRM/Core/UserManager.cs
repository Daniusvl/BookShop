using BookShop.CRM.Core.Base;
using BookShop.CRM.Core.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BookShop.CRM.Core
{
    public class UserManager : IUserManager
    {
        public const string Path = "token.txt";

        private AuthenticatedUser user;
        public AuthenticatedUser User
        {
            get => user ??= new(); 
            set
            {
                user = value;
                Write();
            }
        }

        public UserManager()
        {
            Read();
        }

        public HttpRequestMessage GenerateRequestWithToken(HttpMethod method, string uri)
        {
            HttpRequestMessage message = new(method, uri);
            if (!string.IsNullOrEmpty(User.AccessToken))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", User.AccessToken);
            }
            return message;
        }

        protected virtual void Write()
        {
            string json = JsonConvert.SerializeObject(User);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            File.WriteAllBytes(Path, bytes);
        }

        protected virtual AuthenticatedUser Read()
        {
            if (!File.Exists(Path))
                return User;
            byte[] bytes = File.ReadAllBytes(Path);
            string json = Encoding.UTF8.GetString(bytes);
            User = JsonConvert.DeserializeObject<AuthenticatedUser>(json);
            return User;
        }
    }
}
