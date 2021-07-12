using BookShop.Core;
using BookShop.Core.Abstract;
using BookShop.Identity;
using BookShop.Books;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using BookShop.Features;

namespace BookShop.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDirectories();
            services.AddMapper();
            services.AddMediator();

            services.AddRepositoriesAndBookDb(Configuration);

            services.AddAuthenticationAndAuthorization(Configuration);

            services.AddHttpContextAccessor();
            services.AddScoped<ILoggedInUser, LoggedInUser>();

            services.AddFeatures(Configuration);

            services.AddControllers();

            services.AddSwaggerGen(setup => 
                setup.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Version= "v1",
                    Title = "BookShop API",
                }));

            services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(p => p.WithOrigins("https://localhost:5101").AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "BookShop API"));

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseRouting();

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
