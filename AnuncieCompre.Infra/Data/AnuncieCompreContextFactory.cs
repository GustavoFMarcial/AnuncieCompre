using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AnuncieCompre.Infra.Data;

public class AnuncieCompreContextFactory
    : IDesignTimeDbContextFactory<AnuncieCompreContext>
{
    public AnuncieCompreContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>()
            .Build();

        var connectionString = configuration
            .GetConnectionString("AnuncieCompreContext");

        var optionsBuilder =
            new DbContextOptionsBuilder<AnuncieCompreContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new AnuncieCompreContext(optionsBuilder.Options);
    }
}