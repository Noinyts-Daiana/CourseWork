using CourseWork;
using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { 
    }

    public DbSet<Breed> Breed { get; set; } 
    public DbSet<Animal> Animal { get; set; }
    public DbSet<AdoptAnimal> AdoptAnimal { get; set; }
    public DbSet<Specie> Specie { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Characteristic> Characteristic { get; set; }
    public DbSet<AnimalCharacteristic> AnimalCharacteristic { get; set; }
    public DbSet<MedicalExam> MedicalExam { get; set; }
    public DbSet<Vaccination> Vaccination { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}