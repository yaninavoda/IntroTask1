using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Service;

internal sealed class TeacherService : ITeacherService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;


    public TeacherService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
}
