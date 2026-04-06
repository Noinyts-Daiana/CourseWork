using CourseWork;
using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { 
    }

    public DbSet<Breed> Breeds { get; set; } 
    public DbSet<Animal> Animals { get; set; }
    public DbSet<AdoptAnimal> AdoptAnimals { get; set; }
    public DbSet<Specie> Species { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Characteristic> Characteristics { get; set; }
    public DbSet<AnimalCharacteristic> AnimalCharacteristics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}