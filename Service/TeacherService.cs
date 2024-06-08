using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Service.Contracts;
using Shared.Dtos.TeacherDtos;

namespace Service;

public sealed class TeacherService : ITeacherService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;


    public TeacherService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TeacherShortResponseDto> CreateTeacherAsync(TeacherCreateDto teacherCreateDto)
    {
        var teacher = _mapper.Map<Teacher>(teacherCreateDto);

        _repository.Teacher.CreateTeacher(teacher);
        await _repository.SaveAsync();

        var responseDto = _mapper.Map<TeacherShortResponseDto>(teacher);

        return responseDto;
    }

    public async Task DeleteTeacherAsync(int id, bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(id, trackChanges);

        _repository.Teacher.DeleteTeacher(teacher);
        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<TeacherShortResponseDto>> GetAllTeachersAsync(bool trackChanges)
    {
        var teachers = await _repository.Teacher.GetAllTeachersAsync(trackChanges);

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

        await _repository.SaveAsync();
    }

    public async Task UpdateTeacherAsync(int id, TeacherUpdateDto teacherUpdateDto, bool trackChanges)
    {
        var teacher = await GetTeacherAndCheckIfExists(id, trackChanges);

        _mapper.Map(teacherUpdateDto, teacher);

        await _repository.SaveAsync();
    }

    private async Task<Teacher> GetTeacherAndCheckIfExists(int id, bool trackChanges)
    {
        return await _repository.Teacher.GetTeacherByIdAsync(id, trackChanges)
            ?? throw new TeacherNotFoundException(id);
    }

    private async Task<Course> GetCourseAndCheckIfExists(int id, bool trackChanges)
    {
        return await _repository.Course.GetCourseByIdAsync(id, trackChanges)
            ?? throw new CourseNotFoundException(id);
    }
}
