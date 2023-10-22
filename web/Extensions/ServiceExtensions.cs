using Microsoft.EntityFrameworkCore;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Core.Services;
using Infrastructure.Logger;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Mapper;
using NLog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Web.ActionFilters;
using Marvin.Cache.Headers;

namespace Web.Extensions;

public static class ServiceExtensions
{
    private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    {
        return new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
        .Services.BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>().First();
    }

    public static void AddWebServices(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(config =>
            config.SuppressModelStateInvalidFilter = true
        );

        // Controllers
        services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
            config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            //config.CacheProfiles.Add("MyCachProfile", new CacheProfile() { Duration = 120 });
        }
        ).AddXmlDataContractSerializerFormatters();

        // Cors
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin() //allow any requests from any origin
                .AllowAnyMethod()        //allow origin to send any http request
                .AllowAnyHeader()        //allow origin to add any header to request
                .WithExposedHeaders("Pagination-Metadata"); //make origin able to access this header and read it
            }
            );
        });

        // IIS Configuration
        services.Configure<IISOptions>(options =>
        {

        });

        // Filters
        services.AddScoped<ValidationFilterAttribute>();

        // Respnse Cache
        services.AddResponseCaching();
        services.AddHttpCacheHeaders(
            expirationOptions =>
            {
                expirationOptions.MaxAge = 120;
                expirationOptions.CacheLocation = CacheLocation.Public;
            },
            validationOptions =>
            {
                validationOptions.MustRevalidate = true;
            }
        );
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
