using AutoMapper;
using IntroTask.Entities;
using Shared.Dtos.StudentDtos;

namespace IntroTask.Mapping;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentResponseDto>();
        CreateMap<Student, StudentShortResponseDto>();
        CreateMap<StudentCreateDto, Student>();
        CreateMap<StudentUpdateDto, Student>();
    }
}
