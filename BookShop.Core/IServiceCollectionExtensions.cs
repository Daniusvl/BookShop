using BookShop.Core.AutoMapperProfiles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            return services;
        }
    }
}
