using FormAI.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FormAI.Api.Data;

// Application database context
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // DbSet for storing photos
    public DbSet<Photo> Photos => Set<Photo>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Comentário: só aplica se ninguém configurou ainda (ex.: DI)
        if (!optionsBuilder.IsConfigured)
        {
            // Comentário: fonte da verdade ÚNICA
            var connectionString = "Server=tcp:formai-sql-server.database.windows.net,1433;Initial Catalog=formai;Persist Security Info=False;User ID=formai;Password=Test@ndo123456;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            optionsBuilder.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configura todas as entidades para usar o nome da classe como nome da tabela
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.Entity(entity.Name).ToTable(entity.ClrType.Name);
        }
    }
}
