using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace IntroTaskWebApi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class TeacherCoursesController : ControllerBase
{
    private readonly IServiceManager _service;

    public TeacherCoursesController(IServiceManager service)
    {
        _service = service;
    }
}
