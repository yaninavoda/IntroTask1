using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Service.Contracts;
using Shared.Dtos.CourseDtos;

namespace Service;

public sealed class CourseService : ICourseService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;


    public CourseService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CourseShortResponseDto> CreateCourseAsync(CourseCreateDto courseCreateDto)
    {
        var course = _mapper.Map<Course>(courseCreateDto);

        _repository.Course.CreateCourse(course);

        await _repository.SaveAsync();

        var responseDto = _mapper.Map<CourseShortResponseDto>(course);

        return responseDto;
    }

    public async Task DeleteCourseAsync(int id, bool trackChanges)
    {
        var course = await GetCourseAndCheckIfExists(id, trackChanges);

        _repository.Course.DeleteCourse(course);

        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<CourseShortResponseDto>> GetAllCoursesAsync(bool trackChanges)
    {
        var courses = await _repository.Course.GetAllCoursesAsync(trackChanges);

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

        await _repository.SaveAsync();
    }

    public async Task AppointTeacherForCourse(
        int id,
        int teacherId,
        CourseUpdateDto courseDto,
        bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(teacherId, trackChanges);

        var course = await GetCourseAndCheckIfExists(id, trackChanges);

        teacher.Courses!.Add(course);

        _mapper.Map(courseDto, course);

        course.TeacherId = teacherId;
        course.Teacher = teacher;

        await _repository.SaveAsync();
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

        await _repository.SaveAsync();
    }

    private async Task<Teacher> GetTeacherAndCheckIfExists(int teacherId, bool trackChanges)
    {
        return await _repository.Teacher.GetTeacherByIdAsync(
            teacherId,
            trackChanges)
        ?? throw new TeacherNotFoundException(teacherId);
    }

    private async Task<Student> GetStudentAndCheckIfExists(int studentId, bool trackChanges)
    {
        return await _repository.Student.GetStudentByIdAsync(studentId, trackChanges)
            ?? throw new StudentNotFoundException(studentId);
    }

    private async Task<Course> GetCourseAndCheckIfExists(int id, bool trackChanges)
    {
        return await _repository.Course.GetCourseByIdAsync(id, trackChanges)
            ?? throw new CourseNotFoundException(id);
    }
}
