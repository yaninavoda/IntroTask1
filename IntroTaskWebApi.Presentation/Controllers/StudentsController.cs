using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.Dtos;

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

    [HttpGet("{id:int}", Name = "StudentById")]
    public IActionResult GetStudent(int id)
    {
        var student = _service.StudentService.GetStudentById(id, trackChanges: false);
        return Ok(student);
    }

    [HttpPost]
    public IActionResult CreateStudent([FromBody]StudentCreateDto student)
    {
        if (student is null)
            return BadRequest("StudentCreatDto object is null");

        var createdStudent = _service.StudentService.CreateStudent(student);

        return CreatedAtRoute("StudentById", new { id = createdStudent.Id },
        createdStudent);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteEmployeeForCompany(int id)
    {
        _service.StudentService.DeleteStudent(id, trackChanges: false);

        return NoContent();
    }
}
