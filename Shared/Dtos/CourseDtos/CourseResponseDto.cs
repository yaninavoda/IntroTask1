using Shared.Dtos.TeacherDtos;

namespace Shared.Dtos.CourseDtos;

public record CourseResponseDto(int Id, string Title, TeacherShortResponseDto? Teacher);
