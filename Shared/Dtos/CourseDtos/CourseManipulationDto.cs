using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.CourseDtos;

public abstract record CourseManipulationDto
{
    [Required(ErrorMessage = "{0} is a required field.")]
    [MaxLength(60, ErrorMessage = "Maximum length for the {0} is 60 characters.")]
    public string? Title { get; init; }
}
