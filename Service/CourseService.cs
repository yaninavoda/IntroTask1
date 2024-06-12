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
        var course = await GetCourseAndCheckIfExists(id, trackChanges);

        _repositoryManager.Course.Delete(course);

        await _repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<CourseShortResponseDto>> GetAllCoursesAsync()
    {
        var courses = await _repositoryManager.Course.GetAllAsync();

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

    public async Task UpdateCourseAsync(int id, CourseUpdateDto courseUpdateDto, bool trackChages)
    {
        var course = await GetCourseAndCheckIfExists(id, trackChages);

        _mapper.Map(courseUpdateDto, course);

        await _repositoryManager.SaveAsync();
    }

    public async Task AppointTeacherForCourse(int id, int teacherId, CourseUpdateDto courseDto, bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(teacherId, trackChanges);

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

        var isEnrolled = course.Students.Contains(student);

        if (!isEnrolled)
        {
            throw new StudentCourseNotConnectedException(studentId, id);
        }

        student.Courses.Remove(course);
        course.Students.Remove(student);

        _mapper.Map(courseDto, course);

        await _repositoryManager.SaveAsync();
    }

    private async Task<Teacher> GetTeacherAndCheckIfExists(int id, bool trackChanges)
    {
        return await _repositoryManager.Teacher.GetSingleOrDefaultAsync(
            predicate: t => t.Id == id,
            include: t => t.Include(t => t.Courses),
            trackChanges)

            ?? throw new TeacherNotFoundException(id);
    }

    private async Task<Student> GetStudentAndCheckIfExists(int studentId, bool trackChanges)
    {
        return await _repositoryManager.Student.GetSingleOrDefaultAsync(
            predicate: s => s.Id == studentId,
            include: s => s.Include(s => s.Courses),
            trackChanges)

            ?? throw new StudentNotFoundException(studentId);
    }

    private async Task<Course> GetCourseAndCheckIfExists(int id, bool trackChanges)
    {
        return await _repositoryManager.Course.GetSingleOrDefaultAsync(
            predicate: c => c.Id == id,
            include: c => c
                .Include(c => c.Teacher)
                .Include(c => c.Students),
            trackChanges)

            ?? throw new CourseNotFoundException(id);
    }
}
