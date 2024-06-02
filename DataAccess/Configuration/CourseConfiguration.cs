using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IntroTask.Entities;

namespace DataAccess.Configuration;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder
            .HasData(
            new Course
            {
                Id = 1,
                Title = "Calculus",
                TeacherId = 1,
            },
            new Course
            {
                Id = 2,
                Title = "History",
                TeacherId = 2,
            });

        builder.HasOne(x => x.Teacher)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.TeacherId);
    }
}
