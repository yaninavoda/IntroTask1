using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.Dtos;

namespace IntroTask.Presentation.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IServiceManager _service;

    public StudentsController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var companies = await _service.StudentService.GetAllStudentsAsync(trackChanges: false);

        return Ok(companies);
    }

    [HttpGet("{id:int}", Name = "StudentById")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var student = await _service.StudentService.GetStudentByIdAsync(id, trackChanges: false);

        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody]StudentCreateDto student)
    {
        if (student is null)
            return BadRequest("StudentCreatDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdStudent = await _service.StudentService.CreateStudentAsync(student);

        return CreatedAtRoute("StudentById", new { id = createdStudent.Id },
        createdStudent);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody]StudentUpdateDto student)
    {
        if (student is null)
            return BadRequest("StudentCreatDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.StudentService.UpdateStudentAsync(id, student, trackChanges: true);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        await _service.StudentService.DeleteStudentAsync(id, trackChanges: false);

        return NoContent();
    }
}
