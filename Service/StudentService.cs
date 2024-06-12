using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.Dtos.StudentDtos;

namespace Service;

public sealed class StudentService : IStudentService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public StudentService(IRepositoryManager repository, IMapper mapper)
    {
        _repositoryManager = repository;
        _mapper = mapper;
    }

    public async Task<StudentShortResponseDto> CreateStudentAsync(StudentCreateDto studentCreateDto)
    {
        var student = _mapper.Map<Student>(studentCreateDto);

        _repositoryManager.Student.Create(student);
        await _repositoryManager.SaveAsync();

        var responseDto = _mapper.Map<StudentShortResponseDto>(student);

        return responseDto;
    }

    public async Task DeleteStudentAsync(int id, bool trackChanges)
    {
        var student = await GetStudentAndCheckIfExists(id, trackChanges);

        _repositoryManager.Student.Delete(student);

        await _repositoryManager.SaveAsync();
    }

    public async Task EnrollStudentInCourseAsync(int studentId, int courseId, StudentUpdateDto studentUpdateDto, bool trackChanges)
    {
        var student = await GetStudentAndCheckIfExists(studentId, trackChanges);
        var course = await GetCourseAndCheckIfExists(courseId, trackChanges);

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

        await _repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<StudentShortResponseDto>> GetAllStudentsAsync()
    {
        var students = await _repositoryManager.Student.GetAllAsync();
        var studentDtos = _mapper.Map<List<StudentShortResponseDto>>(students)
            ?? [];

        return studentDtos;
    }

    public async Task<StudentResponseDto> GetStudentByIdAsync(int id, bool trackChanges)
    {
        var student = await GetStudentAndCheckIfExists(id, trackChanges);

        var studentDto = _mapper.Map<StudentResponseDto>(student);

        return studentDto;
    }

    public async Task UpdateStudentAsync(int id, StudentUpdateDto studentUpdateDto, bool trackChanges)
    {
        var student = await GetStudentAndCheckIfExists(id, trackChanges);

        _mapper.Map(studentUpdateDto, student);

        await _repositoryManager.SaveAsync();
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
