using AutoMapper;
using IntroTask.Entities;
using Shared.Dtos.TeacherDtos;

namespace IntroTaskWebApi.Mapping;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        CreateMap<Teacher, TeacherResponseDto>();
        CreateMap<Teacher, TeacherShortResponseDto>();
        CreateMap<TeacherCreateDto, Teacher>();
        CreateMap<TeacherUpdateDto, Teacher>();
    }
}
