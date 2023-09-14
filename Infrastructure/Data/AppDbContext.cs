using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Configuration;

namespace Infrastructure.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    DbSet<Employee>? Employees { get; set; }
    DbSet<Company>? Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }
}
