using DentalHealthFollow_Up.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalHealthFollow_Up.DataAccess.Mappings
{
    public sealed class GoalRecordMap : IEntityTypeConfiguration<GoalRecord>
    {
        public void Configure(EntityTypeBuilder<GoalRecord> e)
        {
            e.ToTable("GoalRecords");

            
            e.Property(p => p.Date).HasColumnName("RecordDateTime");
            e.Property(p => p.DurationInMinutes).HasColumnName("DurationMinutes");
            e.Property(p => p.ImageBase64).HasColumnName("ImagePath");
            e.Property(p => p.Note).HasColumnName("Note");

            
            e.Ignore(p => p.CreatedAt);

            
            e.HasOne(p => p.User)
             .WithMany(u => u.GoalRecords)
             .HasForeignKey(p => p.UserId)
             .OnDelete(DeleteBehavior.Restrict); 

            e.HasOne(p => p.Goal)
             .WithMany(g => g.GoalRecords)
             .HasForeignKey(p => p.GoalId);

            e.HasIndex(p => new { p.UserId, p.Date });
        }
    }
}
