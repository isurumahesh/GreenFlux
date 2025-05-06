using GreenFlux.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreenFlux.Infrastructure.Data
{
    public class GreenFluxDbContext : DbContext
    {
        public GreenFluxDbContext(DbContextOptions<GreenFluxDbContext> options) : base(options)
        {
        }

        public DbSet<ChargeStation> ChargeStations { get; set; }
        public DbSet<Connector> Connectors { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChargeStation>(entity =>
            {
                entity.HasOne(cs => cs.Group)
                      .WithMany(g => g.ChargeStations)
                      .HasForeignKey(cs => cs.GroupId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Connector>(entity =>
            {
                entity.HasKey(c => new { c.Id, c.ChargeStationId });

                entity.HasOne(c => c.ChargeStation)
                      .WithMany(cs => cs.Connectors)
                      .HasForeignKey(c => c.ChargeStationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //   modelBuilder.Seed();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((AuditEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    ((AuditEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}