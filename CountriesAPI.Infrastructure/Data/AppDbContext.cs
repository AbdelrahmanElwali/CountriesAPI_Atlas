using CountriesAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CountriesAPI.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id);

            // عشان نضمن مفيش دولتين بنفس الكود
            entity.HasIndex(e => e.Cca2).IsUnique();
            entity.HasIndex(e => e.Cca3).IsUnique();

            entity.Property(e => e.CommonName).HasMaxLength(200);
            entity.Property(e => e.OfficialName).HasMaxLength(300);
            entity.Property(e => e.Cca2).HasMaxLength(2);
            entity.Property(e => e.Cca3).HasMaxLength(3);
            entity.Property(e => e.Region).HasMaxLength(100);
            entity.Property(e => e.Subregion).HasMaxLength(100);
            entity.Property(e => e.Capital).HasMaxLength(200);
            entity.Property(e => e.FlagEmoji).HasMaxLength(10);
        });
    }
}