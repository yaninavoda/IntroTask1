using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.Dtos.StudentDtos;

namespace IntroTask.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class StudentsController : ControllerBase
{
    private readonly IServiceManager _service;

    public StudentsController(IServiceManager service)
    {
        _service = service;
    }

    /// <summary>
    /// Gets the list of all students.
    /// </summary>
    /// <returns>A list of all students.</returns>
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await _service.StudentService.GetAllStudentsAsync(trackChanges: false);

        return Ok(students);
    }

    /// <summary>
    /// Gets the student with the provided id.
    /// </summary>
    /// <param name="id">the student's to retrieve id</param>
    /// <returns>The student with the provided id from the database.</returns>
    [HttpGet("{id:int}", Name = "StudentById")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var student = await _service.StudentService.GetStudentByIdAsync(id, trackChanges: false);

        return Ok(student);
    }

    /// <summary>
    /// Creates a new student with the data from the request body.
    /// </summary>
    /// <remarks>
    /// Sample request
    /// POST api/students
    /// {
    ///     "firstName": "Jane",
    ///     "lastName": "Doe"
    /// }
    /// </remarks>
    /// <param name="student">StudentCreateDto</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateStudent([FromBody]StudentCreateDto student)
    {
        if (student is null)
            return BadRequest($"{nameof(StudentCreateDto)} object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdStudent = await _service.StudentService.CreateStudentAsync(student);

        return CreatedAtRoute("StudentById", new { id = createdStudent.Id },
        createdStudent);
    }

    /// <summary>
    /// Updates the student with the provided id and data supplied in the request body.
    /// </summary>
    /// <remarks>
    /// Sample request
    /// PUT api/students/1
    /// {
    ///     "firstName": "Jane",
    ///     "lastName": "Doe"
    /// }
    /// </remarks>
    /// <param name="id">the student's to update id</param>
    /// <param name="student">StudentUpdateDto</param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody]StudentUpdateDto student)
    {
        if (student is null)
            return BadRequest($"{nameof(StudentUpdateDto)} object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.StudentService.UpdateStudentAsync(id, student, trackChanges: true);

        return NoContent();
    }
    /// <summary>
    /// Enrolls the student in the course.
    /// </summary>
    /// <param name="id">The id of the student to be enrolled</param>
    /// <param name="courseId">The id of the course to enroll the student in</param>
    /// <param name="student">student dto</param>
    /// <returns></returns>
    [HttpPut("{id:int}/Courses/{courseId:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> EnrollStudentInCourse(int id, int courseId, [FromBody] StudentUpdateDto student)
    {
        if (student is null)
            return BadRequest($"{nameof(StudentUpdateDto)} object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.StudentService.EnrollStudentInCourseAsync(id, courseId, student, trackChanges: true);

        return NoContent();
    }


    /// <summary>
    /// Deletes the student with the provided id from the database.
    /// </summary>
    /// <param name="id">the student's to delete id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        await _service.StudentService.DeleteStudentAsync(id, trackChanges: false);

        return NoContent();
    }
}
