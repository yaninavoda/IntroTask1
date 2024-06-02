using AutoMapper;
using IntroTask.Entities;
using Shared.Dtos.CourseDtos;

namespace IntroTaskWebApi.Mapping;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseResponseDto>();
        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseUpdateDto, Course>();
    }
}
