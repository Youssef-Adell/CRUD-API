using Microsoft.EntityFrameworkCore;
using Core.IRepositories;
using Core.IServices;
using Core.Services;
using Infrastructure.Logger;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;

namespace Company.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }
            );
        });
    }

    public static void ConfigureIISIntegration(this IServiceCollection services)
    {
        services.Configure<IISOptions>(options =>
        {

        }
        );
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerService, NlogLoggerService>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }
    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(optionsbuilder => optionsbuilder.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        // services.AddSqlServer<AppDbContext>(configuration.GetConnectionString("SqlConnection"));
    }
}
