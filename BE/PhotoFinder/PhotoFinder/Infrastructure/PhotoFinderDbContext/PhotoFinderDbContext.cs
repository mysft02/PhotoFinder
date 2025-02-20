using Microsoft.EntityFrameworkCore;
using PhotoFinder.Entity;

namespace PhotoFinder.Infrastructure.Database
{
    public class PhotoFinderDbContext : DbContext
    {
        public PhotoFinderDbContext(DbContextOptions<PhotoFinderDbContext> options) : base(options) { }

        public DbSet<users> Users { get; set; }
        public DbSet<photographers> Photographers { get; set; }
        public DbSet<packages> Packages { get; set; }
        public DbSet<bookings> Bookings { get; set; }
        public DbSet<reviews> Reviews { get; set; }
        public DbSet<payments> Payments { get; set; }
        public DbSet<availability> Availabilities { get; set; }
        public DbSet<notifications> Notifications { get; set; }
        public DbSet<conversations> Conversations { get; set; }
        public DbSet<messages> Messages { get; set; }
        public DbSet<photos> Photos { get; set; }


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

            modelBuilder.Entity<photographers>(entity =>
            {
                entity.HasKey(e => e.photographer_id);
                entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.user_id).OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.bio).HasMaxLength(255);
                entity.Property(e => e.portfolio_url).HasMaxLength(255);
                entity.Property(e => e.rating);
                entity.Property(e => e.location).HasMaxLength(255);
                entity.Property(e => e.created_at).IsRequired();
            });

            modelBuilder.Entity<packages>(entity =>
            {
                entity.HasKey(e => e.package_id);
                entity.HasOne(e => e.Photographer).WithMany(x => x.packages).HasForeignKey(e => e.photographer_id).OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.package_name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.description).IsRequired().HasMaxLength(255);
                entity.Property(e => e.price);
                entity.Property(e => e.duration).HasMaxLength(255);
                entity.Property(e => e.created_at).IsRequired();
            });

            modelBuilder.Entity<bookings>(entity =>
            {
                entity.HasKey(e => e.booking_id);
                entity.HasOne(e => e.Photographer).WithMany(x => x.bookings).HasForeignKey(e => e.photographer_id).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Package).WithMany(x => x.bookings).HasForeignKey(e => e.package_id).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Customer).WithMany(x => x.bookings).HasForeignKey(e => e.customer_id).OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.event_date).IsRequired();
                entity.Property(e => e.event_location).IsRequired();
                entity.Property(e => e.status);
                entity.Property(e => e.total_price);
                entity.Property(e => e.created_at).IsRequired();
                entity.Property(e => e.updated_at).IsRequired();
            });

            modelBuilder.Entity<reviews>(entity =>
            {
                entity.HasKey(e => e.review_id);
                entity.HasOne(e => e.Booking).WithMany().HasForeignKey(e => e.booking_id).OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.rating);
                entity.Property(e => e.comment).IsRequired().HasMaxLength(255);
                entity.Property(e => e.created_at).IsRequired();
            });

            modelBuilder.Entity<payments>(entity =>
            {
                entity.HasKey(e => e.payment_id);
                entity.HasOne(e => e.Booking).WithMany().HasForeignKey(e => e.booking_id).OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.payment_method).IsRequired().HasMaxLength(255);
                entity.Property(e => e.payment_status).IsRequired().HasMaxLength(255);
                entity.Property(e => e.payment_date).IsRequired();
                entity.Property(e => e.amount).HasDefaultValue(0);
            });

            modelBuilder.Entity<availability>(entity =>
            {
                entity.HasKey(e => e.availability_id);
                entity.HasOne(e => e.Photographer).WithMany(x => x.availabilities).HasForeignKey(e => e.photographer_id).OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.date).IsRequired();
                entity.Property(e => e.start_time).IsRequired();
                entity.Property(e => e.end_time).IsRequired();
            });

            modelBuilder.Entity<notifications>(entity =>
            {
                entity.HasKey(e => e.notification_id);
                entity.HasOne(e => e.user).WithMany(x => x.notifications).HasForeignKey(e => e.user_id).OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.message).IsRequired().HasMaxLength(255);
                entity.Property(e => e.is_read).IsRequired().HasDefaultValue(false);
                entity.Property(e => e.created_at).IsRequired();
            });

            modelBuilder.Entity<conversations>(entity =>
            {
                entity.HasKey(e => e.conversation_id);
                entity.HasOne(e => e.customer).WithMany(x => x.conversations).HasForeignKey(e => e.customer_id).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.photographer).WithMany(x => x.conversations).HasForeignKey(e => e.photographer_id).OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.created_at).IsRequired();
                entity.Property(e => e.updated_at).IsRequired();
            });

            modelBuilder.Entity<messages>(entity =>
            {
                entity.HasKey(e => e.message_id);
                entity.HasOne(e => e.conversations).WithMany(x => x.messages).HasForeignKey(e => e.conversation_id).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.senders).WithMany(x => x.messages).HasForeignKey(e => e.sender_id).OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.content).IsRequired().HasMaxLength(255);
                entity.Property(e => e.is_read).IsRequired().HasDefaultValue(false);
                entity.Property(e => e.sent_at).IsRequired();
            });

            modelBuilder.Entity<photos>(entity =>
            {
                entity.HasKey(e => e.photo_id);
                entity.HasOne(e => e.photographer).WithMany(x => x.photos).HasForeignKey(e => e.photographer_id).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.package).WithMany(x => x.photos).HasForeignKey(e => e.package_id).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.booking).WithMany(x => x.photos).HasForeignKey(e => e.booking_id).OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.photo_url).IsRequired().HasMaxLength(255);
                entity.Property(e => e.is_public).IsRequired().HasDefaultValue(false);
                entity.Property(e => e.uploaded_at).IsRequired();
            });
        }
    }
}
