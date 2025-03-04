using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Db
{
    public class ZvuchokContext(DbContextOptions<ZvuchokContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<Sample> Samples => Set<Sample>();
        public DbSet<SignedUrl> SignedUrls => Set<SignedUrl>();
        public DbSet<SamplePack> SamplePacks => Set<SamplePack>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.NormalizedUsername)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Slug)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Navigation(u => u.Credentials)
                .AutoInclude(false);

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Credentials);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedSamplePacks)
                .WithOne(sp => sp.Author)
                .HasForeignKey(sp => sp.AuthorId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedSamples)
                .WithOne(s => s.Author)
                .HasForeignKey(s => s.AuthorId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("DATETIME('now')");

            //

            modelBuilder.Entity<Like>()
                .HasIndex(l => l.SampleId);

            modelBuilder.Entity<Like>()
                .HasIndex(l => l.UserId);

            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.SampleId, l.UserId })
                .IsUnique();

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Sample)
                .WithMany(t => t.Likes)
                .HasForeignKey(l => l.SampleId)
                .IsRequired();

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.FavoriteSamples)
                .HasForeignKey(l => l.UserId)
                .IsRequired();

            modelBuilder.Entity<Like>()
                .Property(l => l.CreatedAt)
                .HasDefaultValueSql("DATETIME('now')");

            //

            modelBuilder.Entity<Sample>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Sample>()
                .HasIndex(s => s.Slug)
                .IsUnique();

            modelBuilder.Entity<Sample>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("DATETIME('now')");

            //

            modelBuilder.Entity<SamplePack>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<SamplePack>()
                .HasIndex(s => s.Slug)
                .IsUnique();

            modelBuilder.Entity<SamplePack>()
                .HasMany(sp => sp.Samples)
                .WithOne(s => s.SamplePack)
                .HasForeignKey(s => s.SamplePackId)
                .IsRequired(false);

            modelBuilder.Entity<SamplePack>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("DATETIME('now')");
        }
    }
}
