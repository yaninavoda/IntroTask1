using IntroTaskWebApi.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.Dtos.UserDtos;

namespace IntroTaskWebApi.Presentation.Controllers;

[Route("api/Authentication")]
[ApiController]
public class AuthenticationController(IServiceManager service) : ControllerBase
{
    private readonly IServiceManager _service = service;

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistrationDto)
    {
        var result = await _service.AuthenticationService.RegisterUser(userRegistrationDto);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost("Login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDto userAuthenticationDto)
    {
        if (!await _service.AuthenticationService.ValidateUser(userAuthenticationDto))
        {
            return Unauthorized(); 
        }

        var tokenDto = await _service.AuthenticationService
            .CreateToken(populateExp: true);

        return Ok(tokenDto);
    }
}
