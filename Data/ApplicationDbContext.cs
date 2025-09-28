
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wger.Api.Models;

namespace Wger.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<GymConfig> GymConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships and constraints here if needed
            builder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne() // Assuming IdentityUser does not have a direct navigation property back to UserProfile
                .HasForeignKey<UserProfile>(up => up.UserId);

            builder.Entity<UserProfile>()
                .HasOne(up => up.Gym)
                .WithMany()
                .HasForeignKey(up => up.GymId);

            builder.Entity<UserProfile>()
                .HasOne(up => up.NotificationLanguage)
                .WithMany()
                .HasForeignKey(up => up.NotificationLanguageId);

            builder.Entity<GymConfig>()
                .HasOne(gc => gc.DefaultGym)
                .WithMany()
                .HasForeignKey(gc => gc.DefaultGymId);
        }
    }
}
