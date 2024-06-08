using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.Dtos.UserDtos;

namespace Service;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }
    public async Task<IdentityResult> RegisterUser(UserRegistrationDto userRegistrationDto)
    {
        var user = _mapper.Map<User>(userRegistrationDto);

        var result = await _userManager.CreateAsync(user, userRegistrationDto.Password!);

        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, userRegistrationDto.Roles);

        return result;
    }
}
