﻿using Shared.Dtos.CourseDtos;

namespace Service.Contracts;

public interface ICourseService
{
    Task<IEnumerable<CourseShortResponseDto>> GetAllCoursesAsync(bool trackChanges);
    Task<CourseResponseDto> GetCourseByIdAsync(int id, bool trackChanges);
    Task<CourseResponseDto> CreateCourseAsync(CourseCreateDto courseCreateDto);
    Task DeleteCourseAsync(int id, bool trackChanges);
    Task UpdateCourseAsync(
        int id,
        CourseUpdateDto courseUpdateDto,
        bool trackChanges);
    Task AppointTeacherForCourse(
        int id,
        int teacherId,
        CourseUpdateDto courseDto,
        bool trackChanges);

    Task ExcludeStudentFromCourse(
        int id,
        int studentId,
        CourseUpdateDto courseDto,
        bool trackChanges);
}
