using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Books.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Books
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoriesAndBookDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BooksDb>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("BooksDb")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
