using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data.ContextFactory;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{

    public AppDbContext CreateDbContext(string[] args)
    {
        // Hard code connection string because we cant access appsettings file from this project
        string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=CompanyEmployees;Integrated Security=true; TrustServerCertificate = true";

        // Create Builder to Configure and Create DbContextOptions to pass it to returned AppDbContext
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder<AppDbContext>()
                                                .UseSqlServer(connectionString);

        DbContextOptions options = builder.Options;

        return new AppDbContext(options);
    }

}
