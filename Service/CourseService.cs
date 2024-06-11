using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Service.Contracts;
using Shared.Dtos.CourseDtos;

namespace Service;

public sealed class CourseService : ICourseService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;


    public CourseService(IRepositoryManager repository, IMapper mapper)
    {
        _repositoryManager = repository;
        _mapper = mapper;
    }

    public async Task<CourseShortResponseDto> CreateCourseAsync(CourseCreateDto courseCreateDto)
    {
        var course = _mapper.Map<Course>(courseCreateDto);

        _repositoryManager.Course.CreateCourse(course);

        await _repositoryManager.SaveAsync();

        var responseDto = _mapper.Map<CourseShortResponseDto>(course);

        return responseDto;
    }

    public async Task DeleteCourseAsync(int id, bool trackChanges)
    {
        var course = await GetCourseAndCheckIfExists(id, trackChanges);

        _repositoryManager.Course.DeleteCourse(course);

        await _repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<CourseShortResponseDto>> GetAllCoursesAsync(bool trackChanges)
    {
        var courses = await _repositoryManager.Course.GetAllCoursesAsync(trackChanges);

        var responseDtos = _mapper.Map<List<CourseShortResponseDto>>(courses)
            ?? [];

        return responseDtos;
    }

    public async Task<CourseResponseDto> GetCourseByIdAsync(int id, bool trackChanges)
    {
        var course = await GetCourseAndCheckIfExists(id, trackChanges);

        var responseDto = _mapper.Map<CourseResponseDto>(course);

        return responseDto;
    }

    public async Task UpdateCourseAsync(int id, CourseUpdateDto courseUpdateDto, bool trackChanges)
    {
        var course = await GetCourseAndCheckIfExists(id, trackChanges);

        _mapper.Map(courseUpdateDto, course);

        await _repositoryManager.SaveAsync();
    }

    public async Task AppointTeacherForCourse(
        int id,
        int teacherId,
        CourseUpdateDto courseDto,
        bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(teacherId);

        var course = await GetCourseAndCheckIfExists(id, trackChanges);

        teacher.Courses!.Add(course);

        _mapper.Map(courseDto, course);

        course.TeacherId = teacherId;
        course.Teacher = teacher;

        await _repositoryManager.SaveAsync();
    }

    public async Task ExcludeStudentFromCourse(int id, int studentId, CourseUpdateDto courseDto, bool trackChanges)
    {
        var student = await GetStudentAndCheckIfExists(studentId, trackChanges);

        var course = await GetCourseAndCheckIfExists(id, trackChanges);

        var isEnrolled = student.Courses.Contains(course);

        if (!isEnrolled)
        {
            throw new StudentCourseNotConnectedException(studentId, id);
        }

        student.Courses.Remove(course);
        course.Students.Remove(student);

        _mapper.Map(courseDto, course);

        await _repositoryManager.SaveAsync();
    }

    private async Task<Teacher> GetTeacherAndCheckIfExists(int id)
    {
        return await _repositoryManager.Teacher.GetSingleOrDefaultAsync(
            predicate: t => t.Id == id)
            ?? throw new TeacherNotFoundException(id);
    }

    private async Task<Student> GetStudentAndCheckIfExists(int studentId, bool trackChanges)
    {
        return await _repositoryManager.Student.GetStudentByIdAsync(studentId, trackChanges)
            ?? throw new StudentNotFoundException(studentId);
    }

    private async Task<Course> GetCourseAndCheckIfExists(int id, bool trackChanges)
    {
        return await _repositoryManager.Course.GetCourseByIdAsync(id, trackChanges)
            ?? throw new CourseNotFoundException(id);
    }
}
