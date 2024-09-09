using Microsoft.EntityFrameworkCore;
using Server.DAL.Entities;

namespace Server.DAL;


public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TelemetryEntity> Telemetries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TelemetryEntity>()
            .HasIndex(t => t.DeviceId);

        modelBuilder.Entity<TelemetryEntity>()
            .HasIndex(t => t.Timestamp);
    }
}
