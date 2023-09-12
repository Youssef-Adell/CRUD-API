using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace Company.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{

    public RepositoryContext CreateDbContext(string[] args)
    {
        // Get Configuration File
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        // Create Builder to Configure and Create DbContextOptions to pass it to returned RepositoryContext
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(configuration.GetConnectionString("SqlConnection"),
                sqlOptionsBuilder => sqlOptionsBuilder.MigrationsAssembly("Company"));

        DbContextOptions options = builder.Options;

        return new RepositoryContext(options);
    }

}
