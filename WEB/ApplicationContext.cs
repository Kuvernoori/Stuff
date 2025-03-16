using Microsoft.EntityFrameworkCore;

namespace WEB;

public class ApplicationContext : DbContext
{
    public DbSet<Stuff> Stuffs { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stuff>().HasData(
            new Stuff { Id = 1, Name = "Pan", Category = "Stable" },
            new Stuff { Id = 2, Name = "Glass", Category = "Fragile" },
            new Stuff { Id = 3, Name = "Gamepad", Category = "Fragile" }
        );
    }
}