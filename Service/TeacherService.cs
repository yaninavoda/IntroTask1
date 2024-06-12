using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.Dtos.TeacherDtos;

namespace Service;

public sealed class TeacherService : ITeacherService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;


    public TeacherService(IRepositoryManager repository, IMapper mapper)
    {
        _repositoryManager = repository;
        _mapper = mapper;
    }

    public async Task<TeacherShortResponseDto> CreateTeacherAsync(TeacherCreateDto teacherCreateDto)
    {
        var teacher = _mapper.Map<Teacher>(teacherCreateDto);

        _repositoryManager.Teacher.Create(teacher);
        await _repositoryManager.SaveAsync();

        var responseDto = _mapper.Map<TeacherShortResponseDto>(teacher);

        return responseDto;
    }

    public async Task DeleteTeacherAsync(int id, bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(id, trackChanges);

        _repositoryManager.Teacher.Delete(teacher);
        await _repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<TeacherShortResponseDto>> GetAllTeachersAsync()
    {
        var teachers = await _repositoryManager.Teacher.GetAllAsync();

        var responseDtos = _mapper.Map<List<TeacherShortResponseDto>>(teachers)
            ?? [];

        return responseDtos;
    }

    public async Task<TeacherResponseDto> GetTeacherByIdAsync(int id, bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(id, trackChanges);

        var responseDto = _mapper.Map<TeacherResponseDto>(teacher);

        return responseDto;
    }

    public async Task ResignTeacherFromCourse(int id, int courseId, TeacherUpdateDto teacherUpdateDto, bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(id, trackChanges);

        var course = await GetCourseAndCheckIfExists(courseId, trackChanges);

        var isTeacherAppointedForCourse = course.TeacherId == id;

        if (!isTeacherAppointedForCourse)
        {
            throw new TeacherCourseNotConnectedException(id, courseId);
        }

        teacher.Courses!.Remove(course);

        _mapper.Map(teacherUpdateDto, teacher);

        await _repositoryManager.SaveAsync();
    }

    public async Task UpdateTeacherAsync(int id, TeacherUpdateDto teacherUpdateDto, bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(id, trackChanges);

        _mapper.Map(teacherUpdateDto, teacher);

        await _repositoryManager.SaveAsync();
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

    private async Task<Teacher> GetTeacherAndCheckIfExists(int id, bool trackChanges)
    {
        return await _repositoryManager.Teacher.GetSingleOrDefaultAsync(
            predicate: t => t.Id == id,
            include: t => t.Include(t => t.Courses),
            trackChanges)

            ?? throw new TeacherNotFoundException(id);
    }
}
