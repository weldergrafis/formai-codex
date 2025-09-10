using FormAI.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FormAI.Api.Data;

// Application database context
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // DbSet for storing photos
    public DbSet<Photo> Photos => Set<Photo>();
}
