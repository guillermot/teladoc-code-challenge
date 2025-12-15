using Microsoft.EntityFrameworkCore;
using Teladoc.DataAccess.Entities;

namespace Teladoc.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}