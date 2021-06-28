using BookShop.Books;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BookShop.Identity;

namespace BookShop.Api
{
    public static class BeforeRun
    {
        public static void ClearDatabase(this IHost host)
        {
            using IServiceScope scope = host.Services.CreateScope();
            BooksDb books_ctx = scope.ServiceProvider.GetRequiredService<BooksDb>();
            books_ctx.Database.EnsureDeleted();
            books_ctx.Database.EnsureCreated();
            IdentityDb identity_ctx = scope.ServiceProvider.GetRequiredService<IdentityDb>();
            identity_ctx.Database.EnsureDeleted();
            identity_ctx.Database.EnsureCreated();
        }
    }
}
