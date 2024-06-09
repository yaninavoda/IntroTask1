using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.Dtos.TokenDtos;
using Shared.Dtos.UserDtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Service;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private User? _user;

    public AuthenticationService(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILoggerManager loggerManager,
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = loggerManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();
        _user.RefreshToken = refreshToken;

        if (populateExp)
        { 
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        }

        await _userManager.UpdateAsync(_user);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto(accessToken, refreshToken);
    }

    public async Task<IdentityResult> RegisterUser(UserRegistrationDto userRegistrationDto)
    {
        var user = _mapper.Map<User>(userRegistrationDto);

        var result = await _userManager.CreateAsync(user, userRegistrationDto.Password!);

        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, userRegistrationDto.Roles);

        return result;
    }

    public async Task<bool> ValidateUser(UserAuthenticationDto userAuthenticationDto)
    {
        _user = await _userManager.FindByNameAsync(userAuthenticationDto.UserName);

        var result = (_user is not null && await _userManager.CheckPasswordAsync(_user, userAuthenticationDto.Password));

        if (!result)
        {
            _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong username or password.");
        }

        return result;
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

        var user = await _userManager.FindByNameAsync(principal.Identity.Name);
        if (user == null || user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new RefreshTokenBadRequest();

        _user = user;

        return await CreateToken(populateExp: false);
    }

    private static SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(_user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["validIssuer"],
            ValidAudience = jwtSettings["validAudience"],
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
