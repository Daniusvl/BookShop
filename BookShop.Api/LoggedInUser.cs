using BookShop.Core.Abstract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BookShop.Api
{
    public class LoggedInUser : ILoggedInUser
    {
        public LoggedInUser(HttpContext context)
        {
            bool contains_token = context.Request.Query.ContainsKey("access_token");

            if (!contains_token)
            {
                UserId = "Unknown";
                return;
            }

            string token = context.Request.Query["access_token"];

            byte[] bytes = Convert.FromBase64String(token.Split('.')[1]);

            string part = Encoding.UTF8.GetString(bytes);

            Dictionary<string, string> claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(part);

            if(!claims.ContainsKey(JwtRegisteredClaimNames.Sub))
            {
                UserId = "Unknown";
                return;
            }
            UserId = claims[JwtRegisteredClaimNames.Sub];
        }

        public string UserId { get; }
    }
}
