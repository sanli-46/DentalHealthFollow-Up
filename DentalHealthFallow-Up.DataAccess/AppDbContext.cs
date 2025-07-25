using DentalHealthFallow_Up.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthFallow_Up.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<GoalRecord> GoalRecords { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<HealthTip> HealthTips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany(u => u.GoalRecords)
                .WithOne(gr => gr.User)
                .HasForeignKey(gr => gr.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.Entity<Goal>()
                .HasMany(g => g.GoalRecords)
                .WithOne(gr => gr.Goal)
                .HasForeignKey(gr => gr.GoalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Goals)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.PasswordResets)
                .WithOne(pr => pr.User)
                .HasForeignKey(pr => pr.Userid);
        }
    }
}

