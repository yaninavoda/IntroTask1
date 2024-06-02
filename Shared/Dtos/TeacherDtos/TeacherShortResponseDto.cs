using Shared.Dtos.CourseDtos;

namespace Shared.Dtos.TeacherDtos;

public record TeacherShortResponseDto(int Id, string Name, List<CourseShortResponseDto> Courses);
