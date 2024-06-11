using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore;
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

        _repositoryManager.Course.Create(course);

        await _repositoryManager.SaveAsync();

        var responseDto = _mapper.Map<CourseShortResponseDto>(course);

        return responseDto;
    }

    public async Task DeleteCourseAsync(int id, bool trackChanges)
    {
        var course = await GetCourseAndCheckIfExists(id);

        _repositoryManager.Course.Delete(course);

        await _repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<CourseShortResponseDto>> GetAllCoursesAsync(bool trackChanges)
    {
        var courses = await _repositoryManager.Course.GetAllAsync();

        var responseDtos = _mapper.Map<List<CourseShortResponseDto>>(courses)
            ?? [];

        return responseDtos;
    }

    public async Task<CourseResponseDto> GetCourseByIdAsync(int id, bool trackChanges)
    {
        var course = await GetCourseAndCheckIfExists(id);

        var responseDto = _mapper.Map<CourseResponseDto>(course);

        return responseDto;
    }

    public async Task UpdateCourseAsync(int id, CourseUpdateDto courseUpdateDto, bool trackChanges)
    {
        var course = await GetCourseAndCheckIfExists(id);

        _mapper.Map(courseUpdateDto, course);

        _repositoryManager.Course.Update(course);

        await _repositoryManager.SaveAsync();
    }

    public async Task AppointTeacherForCourse(
        int id,
        int teacherId,
        CourseUpdateDto courseDto,
        bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(teacherId);

        var course = await GetCourseAndCheckIfExists(id);

        teacher.Courses!.Add(course);

        _mapper.Map(courseDto, course);

        course.TeacherId = teacherId;
        course.Teacher = teacher;
        _repositoryManager.Course.Update(course);
        await _repositoryManager.SaveAsync();
    }

    public async Task ExcludeStudentFromCourse(int id, int studentId, CourseUpdateDto courseDto, bool trackChanges)
    {
        var student = await GetStudentAndCheckIfExists(studentId, trackChanges);

        var course = await GetCourseAndCheckIfExists(id);

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
            predicate: t => t.Id == id,
            include: t => t.Include(t => t.Courses))

            ?? throw new TeacherNotFoundException(id);
    }

    private async Task<Student> GetStudentAndCheckIfExists(int studentId, bool trackChanges)
    {
        return await _repositoryManager.Student.GetStudentByIdAsync(studentId, trackChanges)
            ?? throw new StudentNotFoundException(studentId);
    }

    private async Task<Course> GetCourseAndCheckIfExists(int id)
    {
        return await _repositoryManager.Course.GetSingleOrDefaultAsync(
            predicate: c => c.Id == id,
            include: c => c
                .Include(c => c.Teacher)
                .Include(c => c.Students))

            ?? throw new CourseNotFoundException(id);
    }
}
