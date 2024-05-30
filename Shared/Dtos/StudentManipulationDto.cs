using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos;

public abstract record StudentManipulationDto
{
    [Required(ErrorMessage = "{0} is a required field.")]
    [MaxLength(60, ErrorMessage = "Maximum length for the {0} is 60 characters.")]
    public string? FirstName { get; init; }

    [Required(ErrorMessage = "{0} is a required field.")]
    [MaxLength(60, ErrorMessage = "Maximum length for the {0} is 60 characters.")]
    public string? LastName { get; init; }
}

