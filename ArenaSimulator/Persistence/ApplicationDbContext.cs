using ArenaSimulator.Models;
using Microsoft.EntityFrameworkCore;

namespace ArenaSimulator.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Arena> Arenas { get; set; } = null!;
}