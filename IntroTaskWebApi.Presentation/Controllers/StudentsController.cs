﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.Dtos;

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
    /// Gets the list of all students
    /// </summary>
    /// <returns>A list of all students.</returns>
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await _service.StudentService.GetAllStudentsAsync(trackChanges: false);

        return Ok(students);
    }

    /// <summary>
    /// Gets the student with the provided id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The student with the provided id from the database.</returns>
    [HttpGet("{id:int}", Name = "StudentById")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var student = await _service.StudentService.GetStudentByIdAsync(id, trackChanges: false);

        return Ok(student);
    }

    /// <summary>
    /// Creates a new student with the data from the request body
    /// </summary>
    /// <remarks>
    /// Sample request
    /// POST api/students
    /// {
    ///     "firstName": "Jane",
    ///     "lastName": "Doe"
    /// }
    /// </remarks>
    /// <param name="student"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateStudent([FromBody]StudentCreateDto student)
    {
        if (student is null)
            return BadRequest("StudentCreateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdStudent = await _service.StudentService.CreateStudentAsync(student);

        return CreatedAtRoute("StudentById", new { id = createdStudent.Id },
        createdStudent);
    }

    /// <summary>
    /// Updates the student with the provided id and data supplied in the request body
    /// </summary>
    /// <remarks>
    /// Sample request
    /// PUT api/students
    /// {
    ///     "firstName": "Jane",
    ///     "lastName": "Doe"
    /// }
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="student"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody]StudentUpdateDto student)
    {
        if (student is null)
            return BadRequest("StudentCreateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.StudentService.UpdateStudentAsync(id, student, trackChanges: true);

        return NoContent();
    }

    /// <summary>
    /// Deletes the student with the provided id from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        await _service.StudentService.DeleteStudentAsync(id, trackChanges: false);

        return NoContent();
    }
}
