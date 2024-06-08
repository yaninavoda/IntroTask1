
using Microsoft.AspNetCore.Identity;
using Shared.Dtos.UserDtos;

namespace Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserRegistrationDto userRegistrationDto);
}
