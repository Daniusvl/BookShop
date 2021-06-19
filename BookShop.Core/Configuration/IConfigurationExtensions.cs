using Microsoft.Extensions.Configuration;

namespace BookShop.Core.Configuration
{
    public static class IConfigurationExtensions
    {
        private const string enviroment = "ASPNETCORE_ENVIRONMENT";

        private const string development = "Development";

        public static bool IsDevelopment(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(enviroment) == development;
        }
    }
}
