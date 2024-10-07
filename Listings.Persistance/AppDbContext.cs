using Listings.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Listings.Persistance
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Listings.Domain.Models.Listing> Listings { get; set; }
        public DbSet<SavedListing> SavedListings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the SavedListing as a many-to-many relationship
            modelBuilder.Entity<SavedListing>()
                .HasKey(sl => new { sl.UserId, sl.ListingId });

            // Configure one-to-many relationship between User and SavedListings
            modelBuilder.Entity<SavedListing>()
                .HasOne(sl => sl.User)
                .WithMany(u => u.SavedListings)
                .HasForeignKey(sl => sl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Listing and SavedListings
            modelBuilder.Entity<SavedListing>()
                .HasOne(sl => sl.Listing)
                .WithMany(l => l.SavedByUsers)
                .HasForeignKey(sl => sl.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}