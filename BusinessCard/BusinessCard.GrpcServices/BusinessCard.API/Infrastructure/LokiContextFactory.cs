using System.Reflection;
using Microsoft.EntityFrameworkCore.Design;

namespace BusinessCard.API.Infrastructure;

public class LokiContextFactory : IDesignTimeDbContextFactory<LokiContext>
{
    public LokiContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<LokiContext>();
        optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly(Assembly.GetAssembly(typeof(LokiContext))?.GetName().Name));

        return new LokiContext(optionsBuilder.Options, new NoMediator(), null);
    }
}