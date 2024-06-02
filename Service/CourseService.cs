using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Service.Contracts;
using Shared.Dtos.CourseDtos;
using Shared.Dtos.TeacherDtos;

namespace Service;

internal sealed class CourseService : ICourseService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;


    public CourseService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CourseResponseDto> CreateCourseAsync(CourseCreateDto courseCreateDto)
    {
        var teacherId = courseCreateDto.TeacherId.Value;
        var teacher = await _repository.Teacher.GetTeacherByIdAsync(teacherId, trackChanges: true)
            ?? throw new TeacherNotFoundException(teacherId);

        var course = _mapper.Map<Course>(courseCreateDto);

        _repository.Course.CreateCourse(course);

        await _repository.SaveAsync();

        var responseDto = new CourseResponseDto(course.Id, course.Title, _mapper.Map<TeacherResponseDto>(teacher));
        
        return responseDto;
    }

    public async Task DeleteCourseAsync(int id, bool trackChanges)
    {
        var course = await _repository.Course.GetCourseByIdAsync(id, trackChanges)
            ?? throw new CourseNotFoundException(id);

        _repository.Course.DeleteCourse(course);

        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync(bool trackChanges)
    {
        var courses = await _repository.Course.GetAllCoursesAsync(trackChanges);
        var responseDtos = _mapper.Map<IEnumerable<CourseResponseDto>>(courses);

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

        var teacherId = courseUpdateDto.TeacherId.Value;
        var teacher = await _repository.Teacher.GetTeacherByIdAsync(teacherId, trackChanges: true)
            ?? throw new TeacherNotFoundException(teacherId);

        _mapper.Map(courseUpdateDto, course);

        await _repository.SaveAsync();
    }
}
