using DentalHealthFollow_Up.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthFollow_Up.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Goal> Goals => Set<Goal>();
        public DbSet<GoalRecord> GoalRecords => Set<GoalRecord>();
        public DbSet<PasswordReset> PasswordResets => Set<PasswordReset>();
        public DbSet<HealthTip> HealthTips => Set<HealthTip>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User - Goal
            modelBuilder.Entity<User>()
                .HasMany(u => u.Goals)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - PasswordReset
            modelBuilder.Entity<User>()
                .HasMany(u => u.PasswordResets)
                .WithOne(pr => pr.User)
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Goal - GoalRecord
            modelBuilder.Entity<Goal>()
                .HasMany(g => g.GoalRecords)
                .WithOne(gr => gr.Goal)
                .HasForeignKey(gr => gr.GoalId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - GoalRecord 
            modelBuilder.Entity<User>()
                .HasMany(u => u.GoalRecords)
                .WithOne(gr => gr.User)
                .HasForeignKey(gr => gr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            
            base.OnModelCreating(modelBuilder);
        }
    }
}
