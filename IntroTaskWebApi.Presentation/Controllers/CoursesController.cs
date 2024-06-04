using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.Dtos.CourseDtos;


namespace IntroTaskWebApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CoursesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CoursesController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the list of all courses.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _service.CourseService.GetAllCoursesAsync(trackChanges: false);

            return Ok(courses);
        }

        /// <summary>
        /// Gets the course with the provided id.
        /// </summary>
        /// <param name="id">The course's to retrieve id</param>
        /// <returns>The course with the provided id from the database.</returns>
        [HttpGet("{id:int}", Name = "CourseById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _service.CourseService.GetCourseByIdAsync(id, trackChanges: false);

            return Ok(course);
        }

        /// <summary>
        /// Creates a new course with the data from the request body.
        /// </summary>
        /// <remarks>
        /// Sample request
        /// POST api/courses
        /// {
        ///     "title": "Literature"
        /// }
        /// </remarks>
        /// <param name="course">CourseCreateDto</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto course)
        {
            if (course is null)
                return BadRequest($"{nameof(CourseCreateDto)} object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var createdCourse = await _service.CourseService.CreateCourseAsync(course);

            return CreatedAtRoute("CourseById", new { id = createdCourse.Id },
            createdCourse);
        }

        /// <summary>
        /// Updates the course with the provided id and data supplied in the request body.
        /// </summary>
        /// <remarks>
        /// Sample request
        /// PUT api/courses
        /// {
        ///     "title": "Literature"
        /// }
        /// </remarks>
        /// <param name="id">The course's to update id</param>
        /// <param name="course">CourseUpdateDto</param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseUpdateDto course)
        {
            if (course is null)
                return BadRequest($"{nameof(CourseCreateDto)} object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.CourseService.UpdateCourseAsync(id, course, trackChanges: true);

            return NoContent();
        }
        /// <summary>
        /// Appoints the teacher to the course.
        /// </summary>
        /// <param name="id">The course's id</param>
        /// <param name="teacherId">The id of the teacher to be appointed for this course</param>
        /// <param name="course">CourseUpdateDto</param>
        /// <returns></returns>
        [HttpPut("{id:int}/Teachers/{teacherId:int}")]
        public async Task<IActionResult> AppointTeacherForCourse(int id, int teacherId, [FromBody] CourseUpdateDto course)
        {
            if (course is null)
                return BadRequest($"{nameof(CourseUpdateDto)} object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.CourseService.AppointTeacherForCourse(id, teacherId, course, trackChanges: true);

            return NoContent();
        }

        /// <summary>
        /// Excludes the student from the course.
        /// </summary>
        /// <param name="id">The id of the course</param>
        /// <param name="studentId">The id of the student to be excluded</param>
        /// <param name="course">CourseUpdateDto</param>
        /// <returns></returns>
        [HttpPut("{id:int}/Students/{studentId:int}")]
        public async Task<IActionResult> ExcludeStudentFromCourse(int id, int studentId, [FromBody] CourseUpdateDto course)
        {
            if (course is null)
                return BadRequest($"{nameof(CourseUpdateDto)} object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.CourseService.ExcludeStudentFromCourse(id, studentId, course, trackChanges: true);

            return NoContent();
        }

        /// <summary>
        /// Deletes the course with the provided id from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The course's to delete id</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await _service.CourseService.DeleteCourseAsync(id, trackChanges: false);

            return NoContent();
        }
    }
}
