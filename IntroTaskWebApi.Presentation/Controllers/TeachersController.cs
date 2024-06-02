using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace IntroTaskWebApi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class TeachersController : ControllerBase
{
    private readonly IServiceManager _service;

    public TeachersController(IServiceManager service)
    {
        _service = service;
    }

    /// <summary>
    /// Gets the list of all teachers.
    /// </summary>
    /// <returns>A list of all teachers.</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetTeachers()
    {
        var teachers = await _service.TeacherService.GetAllTeachersAsync(trackChanges: false);

        return Ok(teachers);
    }

    /// <summary>
    /// Gets the teacher with the provided id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The teacher with the provided id from the database.</returns>
    [HttpGet("{id:int}", Name = "TeacherById")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetTeacher(int id)
    {
        var teacher = await _service.TeacherService.GetTeacherByIdAsync(id, trackChanges: false);

        return Ok(teacher);
    }
}
