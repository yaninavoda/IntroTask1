using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Service.Contracts;
using Shared.Dtos.CourseDtos;

namespace Service;

internal sealed class CourseService : ICourseService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;


    public CourseService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CourseResponseDto> CreateCourseAsync(CourseCreateDto courseCreateDto)
    {
        var course = _mapper.Map<Course>(courseCreateDto);

        _repository.Course.CreateCourse(course);

        await _repository.SaveAsync();

        var responseDto = _mapper.Map<CourseResponseDto>(course);

        return responseDto;
    }

    public async Task DeleteCourseAsync(int id, bool trackChanges)
    {
        var course = await _repository.Course.GetCourseByIdAsync(id, trackChanges)
            ?? throw new CourseNotFoundException(id);

        _repository.Course.DeleteCourse(course);

        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<CourseShortResponseDto>> GetAllCoursesAsync(bool trackChanges)
    {
        var courses = await _repository.Course.GetAllCoursesAsync(trackChanges);
        var responseDtos = _mapper.Map<IEnumerable<CourseShortResponseDto>>(courses);

        return responseDtos;
    }

    public async Task<CourseResponseDto> GetCourseByIdAsync(int id, bool trackChanges)
    {
        var course = await _repository.Course.GetCourseByIdAsync(id, trackChanges)
            ?? throw new CourseNotFoundException(id);

        var responseDto = _mapper.Map<CourseResponseDto>(course);

        return responseDto;
    }

    public async Task UpdateCourseAsync(int id, CourseUpdateDto courseUpdateDto, bool trackChanges)
    {
        var course = await _repository.Course.GetCourseByIdAsync(id, trackChanges)
            ?? throw new CourseNotFoundException(id);

        _mapper.Map(courseUpdateDto, course);

        await _repository.SaveAsync();
    }

    public async Task AppointTeacherForCourse(
        int id,
        int teacherId,
        CourseUpdateDto courseDto,
        bool trackChanges)
    {
        var teacher = await _repository.Teacher.GetTeacherByIdAsync(
            teacherId,
            trackChanges)
        ?? throw new TeacherNotFoundException(teacherId);

        var course = await _repository.Course.GetCourseByIdAsync(
            id,
            trackChanges)
        ?? throw new CourseNotFoundException(id);

        teacher.Courses!.Add(course);

        _mapper.Map(courseDto, course);
        course.TeacherId = teacherId;
        course.Teacher = teacher;

        await _repository.SaveAsync();
    }
}
