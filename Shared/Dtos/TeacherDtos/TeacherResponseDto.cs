using Shared.Dtos.CourseDtos;

namespace Shared.Dtos.TeacherDtos;

public record TeacherResponseDto(int Id, string Name, List<CourseResponseDto> Courses);
