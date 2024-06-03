using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.CourseDtos;

public record CourseAppointTeacherDto : CourseManipulationDto
{
    [Range(1, int.MaxValue, ErrorMessage = "{0} must be a positive number.")]
    public int TeacherId { get; set; }
}
