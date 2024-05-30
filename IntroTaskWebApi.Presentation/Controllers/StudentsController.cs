using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace IntroTask.Presentation.Controllers;

[Route("api/students")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IServiceManager _service;

    public StudentsController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetStudents()
    {
        var companies = _service.StudentService.GetAllStudents(trackChanges: false);

        return Ok(companies);
    }
}
