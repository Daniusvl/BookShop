using BookShop.Core.Abstract.Features.EmailSender;
using BookShop.Core.Abstract.Features.FileUploader;
using BookShop.Features.Configuration;
using BookShop.Features.EmailSender;
using BookShop.Features.FileUploader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Features
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Email>(configuration.GetSection("Email"));
            services.AddScoped<IEmailSender, GmailEmailSender>();

            services.AddScoped<BaseFileUploader, PngFileUploader>();
            services.AddScoped<BaseFileUploader, PdfFileUploader>();

            return services;
        }
    }
}
