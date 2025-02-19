using Microsoft.EntityFrameworkCore;
using PhotoFinder.Entity;

namespace PhotoFinder.Infrastructure.Database
{
    public class PhotoFinderDbContext : DbContext
    {
        public PhotoFinderDbContext(DbContextOptions<PhotoFinderDbContext> options) : base(options) { }

        public DbSet<users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set precision for decimal types
            var decimalProps = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(10);
                property.SetScale(2);
            }

            modelBuilder.Entity<users>(entity =>
            {
                entity.HasKey(e => e.user_id);
                entity.HasIndex(e => e.email).IsUnique();
                entity.Property(e => e.email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.password).IsRequired();
                entity.Property(e => e.phone_number).HasMaxLength(11);
                entity.Property(e => e.profile_picture).IsRequired(false);
                entity.Property(e => e.created_at).IsRequired();
                entity.Property(e => e.updated_at).IsRequired();
            });
        }
    }
}
