using Shared.Dtos.StudentDtos;
using Shared.Dtos.TeacherDtos;

namespace Shared.Dtos.CourseDtos;

public record CourseResponseDto(int Id, string Title, TeacherShortResponseDto? Teacher, List<StudentShortResponseDto>? Students);
