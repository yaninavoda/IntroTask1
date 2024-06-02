using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Service.Contracts;
using Shared.Dtos.StudentDtos;

namespace Service;

internal sealed class StudentService : IStudentService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public StudentService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<StudentResponseDto> CreateStudentAsync(StudentCreateDto studentCreateDto)
    {
        var student = _mapper.Map<Student>(studentCreateDto);

        _repository.Student.CreateStudent(student);
        await _repository.SaveAsync();

        var responseDto = _mapper.Map<StudentResponseDto>(student);

        return responseDto;
    }

    public async Task DeleteStudentAsync(int id, bool trackChanges)
    {
        var student = await _repository.Student.GetStudentByIdAsync(id, trackChanges)
            ?? throw new StudentNotFoundException(id);

        _repository.Student.DeleteStudent(student);
        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync(bool trackChanges)
    {
        var students = await _repository.Student.GetAllStudentsAsync(trackChanges);
        var studentDtos = _mapper.Map<IEnumerable<StudentResponseDto>>(students);

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
