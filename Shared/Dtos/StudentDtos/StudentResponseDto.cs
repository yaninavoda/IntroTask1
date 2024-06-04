using Shared.Dtos.CourseDtos;

namespace Shared.Dtos.StudentDtos;

public record StudentResponseDto(int Id, string FirstName, string LastName, List<CourseShortResponseDto> Courses);

