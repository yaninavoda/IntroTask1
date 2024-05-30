using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Service.Contracts;
using Shared.Dtos;

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

    public StudentResponseDto CreateStudent(StudentCreateDto studentCreateDto)
    {
        var student = _mapper.Map<Student>(studentCreateDto);

        _repository.Student.CreateStudent(student);
        _repository.Save();

        var responseDto = _mapper.Map<StudentResponseDto>(student);

        return responseDto;
    }

    public void DeleteStudent(int id, bool trackChanges)
    {
        var student = _repository.Student.GetStudentById(id, trackChanges)
            ?? throw new StudentNotFoundException(id);

        _repository.Student.DeleteStudent(student);
        _repository.Save();
    }

    public IEnumerable<StudentResponseDto> GetAllStudents(bool trackChanges)
    {
        var students = _repository.Student.GetAllStudents(trackChanges);
        var studentDtos = _mapper.Map<IEnumerable<StudentResponseDto>>(students);

        return studentDtos;
    }

    public StudentResponseDto GetStudentById(int id, bool trackChanges)
    {
        var student = _repository.Student.GetStudentById(id, trackChanges)
            ?? throw new StudentNotFoundException(id);

        var studentDto = _mapper.Map<StudentResponseDto>(student);

        return studentDto;
    }

    public void UpdateStudent(int id, StudentUpdateDto studentUpdateDto, bool trackChanges)
    {
        var student = _repository.Student.GetStudentById(id, trackChanges)
            ?? throw new StudentNotFoundException(id);

        _mapper.Map(studentUpdateDto, student);
        _repository.Save();
    }
}
