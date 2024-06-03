using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;

namespace Service;

public class CourseStudentService : ICourseStudentService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CourseStudentService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    /// <summary>
    /// Adds the student to the course and the course to the student.
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task EnrollStudentInCourseAsync(int studentId, int courseId, bool trackChanges)
    {
        var student = await _repository.Student.GetStudentByIdAsync(studentId, trackChanges)
            ?? throw new StudentNotFoundException(studentId);

        var course = await _repository.Course.GetCourseByIdAsync(courseId, trackChanges)
            ?? throw new CourseNotFoundException(courseId);

        var isAlreadyEnrolled = student.Courses.Any(c => c.Id == courseId);

        if (!isAlreadyEnrolled)
        {
            // _repository.
        }
            //if (! isAlreadyEnrolled)
            //{
            //    student.Courses.Add(course);
            //}

            //var doesCourseHaveStudent = course.Students.Any(s => s.Id == studentId);

            //if (! doesCourseHaveStudent)
            //{
            //    course.Students.Add(student);
            //}

            await _repository.SaveAsync();
    }
}
