using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Service.Contracts;
using Shared.Dtos.StudentDtos;

namespace Service;

public sealed class StudentService : IStudentService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public StudentService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StudentShortResponseDto> CreateStudentAsync(StudentCreateDto studentCreateDto)
    {
        var student = _mapper.Map<Student>(studentCreateDto);

        _repository.Student.CreateStudent(student);
        await _repository.SaveAsync();

        var responseDto = _mapper.Map<StudentShortResponseDto>(student);

        return responseDto;
    }

    public async Task DeleteStudentAsync(int id, bool trackChanges)
    {
        var student = await _repository.Student.GetStudentByIdAsync(id, trackChanges)
            ?? throw new StudentNotFoundException(id);

        _repository.Student.DeleteStudent(student);
        await _repository.SaveAsync();
    }

    public async Task EnrollStudentInCourseAsync(int studentId, int courseId, StudentUpdateDto studentUpdateDto, bool trackChanges)
    {
        var student = await _repository.Student.GetStudentByIdAsync(studentId, trackChanges)
            ?? throw new StudentNotFoundException(studentId);

        var course = await _repository.Course.GetCourseByIdAsync(courseId, trackChanges)
            ?? throw new CourseNotFoundException(courseId);

        var isAlreadyEnrolled = student.Courses.Contains(course);

        if (isAlreadyEnrolled)
        {
            throw new StudentCourseAlreadyConnectedException(
                studentId,
                courseId);
        }

        student.Courses.Add(course);
        course.Students.Add(student);

        _mapper.Map(studentUpdateDto, student);

        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<StudentShortResponseDto>> GetAllStudentsAsync(bool trackChanges)
    {
        var students = await _repository.Student.GetAllStudentsAsync(trackChanges);
        var studentDtos = _mapper.Map<List<StudentShortResponseDto>>(students)
            ?? [];

        return studentDtos;
    }

    public async Task<StudentResponseDto> GetStudentByIdAsync(int id, bool trackChanges)
    {
        var student = await _repository.Student.GetStudentByIdAsync(id, trackChanges)
            ?? throw new StudentNotFoundException(id);

        var studentDto = _mapper.Map<StudentResponseDto>(student);

        return studentDto;
    }

    public async Task UpdateStudentAsync(int id, StudentUpdateDto studentUpdateDto, bool trackChanges)
    {
        var student = await _repository.Student.GetStudentByIdAsync(id, trackChanges)
            ?? throw new StudentNotFoundException(id);

        _mapper.Map(studentUpdateDto, student);
        await _repository.SaveAsync();
    }
}
