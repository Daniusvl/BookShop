using BookShop.Books;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Api
{
    public static class BeforeRun
    {
        public static void ClearDatabase(this IHost host)
        {
            using IServiceScope scope = host.Services.CreateScope();
            BooksDb ctx = scope.ServiceProvider.GetRequiredService<BooksDb>();
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
        }
    }
}
