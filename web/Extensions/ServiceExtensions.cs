using Microsoft.EntityFrameworkCore;
using Core.IRepositories;
using Core.IServices;
using Core.Services;
using Infrastructure.Logger;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Mapper;
using NLog;

namespace Company.Extensions;

public static class ServiceExtensions
{
    public static void AddWebServices(this IServiceCollection services)
    {
        // Controllers
        services.AddControllers();

        // Cors
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

        // IIS Configuration
        services.Configure<IISOptions>(options =>
        {

        });
    }

    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Logger
        LogManager.Setup().LoadConfigurationFromFile();
        services.AddSingleton<NLog.ILogger>(LogManager.GetCurrentClassLogger());
        services.AddSingleton<ILoggerService, NlogService>();

        // Mapper
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddSingleton<IMapperService, AutoMapperService>();

        // Data Access
        services.AddDbContext<AppDbContext>(optionsbuilder => optionsbuilder.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        services.AddScoped<IRepositoryManager, EFRepositoryManager>();
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

}
