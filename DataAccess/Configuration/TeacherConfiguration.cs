using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IntroTask.Entities;

namespace DataAccess.Configuration;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder
            .HasData(
                new Teacher
                {
                    Id = 1,
                    Name = "Ken Berry",
                },
                new Teacher
                {
                    Id = 2,
                    Name = "Anthony Chaffee",
                });
    }
}
