using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AnuncieCompre.Infra.Data;

public class AnuncieCompreContextFactory : IDesignTimeDbContextFactory<AnuncieCompreContext>
{
    public AnuncieCompreContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AnuncieCompreContext>();

        options.UseNpgsql("...");

        return new AnuncieCompreContext(options.Options);
    }
}