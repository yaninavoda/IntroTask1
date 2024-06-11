using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
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
        var teacher = await GetTeacherAndCheckIfExists(id);

        _repositoryManager.Teacher.Delete(teacher);
        await _repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<TeacherShortResponseDto>> GetAllTeachersAsync(bool trackChanges)
    {
        var teachers = await _repositoryManager.Teacher.GetAllAsync();

        var responseDtos = _mapper.Map<List<TeacherShortResponseDto>>(teachers)
            ?? [];

        return responseDtos;
    }

    public async Task<TeacherResponseDto> GetTeacherByIdAsync(int id, bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(id);

        var responseDto = _mapper.Map<TeacherResponseDto>(teacher);

        return responseDto;
    }

    public async Task ResignTeacherFromCourse(int id, int courseId, TeacherUpdateDto teacherUpdateDto, bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(id);

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
        var teacher = await GetTeacherAndCheckIfExists(id);

        _mapper.Map(teacherUpdateDto, teacher);

        _repositoryManager.Teacher.Update(teacher);

        await _repositoryManager.SaveAsync();
    }

    private async Task<Teacher> GetTeacherAndCheckIfExists(int id)
    {
        return await _repositoryManager.Teacher.GetSingleOrDefaultAsync(
            predicate: t => t.Id == id)
            ?? throw new TeacherNotFoundException(id);
    }

    private async Task<Course> GetCourseAndCheckIfExists(int id, bool trackChanges)
    {
        return await _repositoryManager.Course.GetCourseByIdAsync(id, trackChanges)
            ?? throw new CourseNotFoundException(id);
    }
}
