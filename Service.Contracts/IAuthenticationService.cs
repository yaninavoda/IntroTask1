
using Microsoft.AspNetCore.Identity;
using Shared.Dtos.TokenDtos;
using Shared.Dtos.UserDtos;

namespace Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserRegistrationDto userRegistrationDto);
    Task<bool> ValidateUser(UserAuthenticationDto userForAuth);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}
