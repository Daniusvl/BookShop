using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace BookShop.Core
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }

        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            return services;
        }

        public static IServiceCollection ConfigureDirectories(this IServiceCollection services)
        {
            if(!Directory.Exists(Directory.GetCurrentDirectory() + @"\Photos"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Photos");
            }

            if(!Directory.Exists(Directory.GetCurrentDirectory() + @"\Books"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Books");
            }
            return services;
        }
    }
}
