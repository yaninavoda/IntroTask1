using IntroTaskWebApi.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.Dtos.TeacherDtos;

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
        var teachers = await _service.TeacherService.GetAllTeachersAsync();

        return Ok(teachers);
    }

    /// <summary>
    /// Gets the teacher with the provided id.
    /// </summary>
    /// <param name="id">The teacher's to retrieve id</param>
    /// <returns>The teacher with the provided id from the database.</returns>
    [HttpGet("{id:int}", Name = "TeacherById")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetTeacher(int id)
    {
        var teacher = await _service.TeacherService.GetTeacherByIdAsync(id, false);

        return Ok(teacher);
    }

    /// <summary>
    /// Creates a new teacher with the data from the request body.
    /// </summary>
    /// <remarks>
    /// Sample request
    /// POST api/courses
    /// {
    ///     "name": "John Smith"
    /// }
    /// </remarks>
    /// <param name="teacher">TeacherCreateDto</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateTeacher([FromBody] TeacherCreateDto teacher)
    {
        var createdTeacher = await _service.TeacherService.CreateTeacherAsync(teacher);

        return CreatedAtRoute("TeacherById", new { id = createdTeacher.Id },
        createdTeacher);
    }

    /// <summary>
    /// Updates the teacher with the provided id and data supplied in the request body.
    /// </summary>
    /// <remarks>
    /// Sample request
    /// PUT api/courses
    /// {
    ///     "name": "John Smith"
    /// }
    /// </remarks>
    /// <param name="id">The teacher's to update id</param>
    /// <param name="teacher">TeacherUpdateDto</param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherUpdateDto teacher)
    {
        await _service.TeacherService.UpdateTeacherAsync(id, teacher, true);

        return NoContent();
    }

    /// <summary>
    /// Resigns the teacher from the course.
    /// </summary>
    /// <param name="id">The teacher's to resign id</param>
    /// <param name="courseId">The id of the course to resign the teacher from</param>
    /// <param name="teacher">TeacherUpdateDto</param>
    /// <returns></returns>
    [HttpPut("{id:int}/Courses/{courseId:int}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> ResignTeacherFromCourse(int id, int courseId, [FromBody] TeacherUpdateDto teacher)
    {
        await _service.TeacherService.ResignTeacherFromCourse(id, courseId, teacher, true);

        return NoContent();
    }

    /// <summary>
    /// Deletes the teacher with the provided id from the database.
    /// </summary>
    /// <param name="id">The teacher's to delete id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        await _service.TeacherService.DeleteTeacherAsync(id, false);

        return NoContent();
    }
}
