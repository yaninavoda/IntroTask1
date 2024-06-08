using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.UserDtos;

public record UserAuthenticationDto
{
    [Required(ErrorMessage = "{0} is required")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "{0} is required")]
    public string? Password { get; init; }   
}
