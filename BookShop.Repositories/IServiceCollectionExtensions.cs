using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Core.Configuration;
using BookShop.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Repositories
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            services.AddScoped(typeof(IAsyncLinqHelper<>), typeof(AsyncLinqHelper<>));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
            services.AddScoped<IBookPhotoRepository, BookPhotoRepository>();
            return services;
        }

        public static IServiceCollection AddACtx(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookDb>(options =>
            {
                if (configuration.IsDevelopment())
                {
                    options.UseInMemoryDatabase("BooksDb");
                }
                else
                {
                    options.UseSqlServer(configuration.GetConnectionString("BooksDb"));
                }
            });
            return services;
        }
    }
}
