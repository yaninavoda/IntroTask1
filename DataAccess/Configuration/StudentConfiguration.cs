using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IntroTask.Entities;

namespace DataAccess.Configuration;
public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
            .HasData(
                new Student
                {
                    Id = 1,
                    FirstName = "Mary",
                    LastName = "Ostin",
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Alice",
                    LastName = "Morgan"
                });
    }
}
