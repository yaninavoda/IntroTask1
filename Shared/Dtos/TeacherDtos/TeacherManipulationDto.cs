using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.TeacherDtos;

public abstract record TeacherManipulationDto
{
    [Required(ErrorMessage = "{0} is a required field.")]
    [MaxLength(60, ErrorMessage = "Maximum length for the {0} is 60 characters.")]
    public string? Name { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "{0} must be a positive number.")]
    [Display(Name = "Course Id")]
    public int? CourseId { get; init; }
}
