using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.TeacherDtos;

namespace IntroTask.Tests.ServiceTests.TeacherServiceTests;

public class ResignTeacherFromCourseTests
{
    private Mock<IRepositoryManager> _repositoryMock;
    private Mock<IMapper> _mapperMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
    }

    private static TeacherUpdateDto GetTeacherUpdateDto()
    {
        return new()
        {
            Name = "John Smith"
        };
    }

    private void SetupMocksPositiveScenario()
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetTeacherByIdAsync(
                            It.IsAny<int>(),
                            It.IsAny<bool>()))
                                .ReturnsAsync(GetTeacher());

        _repositoryMock.Setup(repo => repo.Course.GetCourseByIdAsync(
            It.IsAny<int>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetCourse());

        _mapperMock.Setup(m => m.Map(It.IsAny<TeacherUpdateDto>(),
            It.IsAny<Teacher>())).Returns(GetTeacher());

        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
    }

    private static Teacher GetTeacher()
    {
        return new Teacher
        {
            Id = 1,
            Name = "John Smith",
            Courses = new List<Course>
            {
                new()
                { Id = 1,
                    Title = "Course 1",
                    TeacherId = 1
                }
            }
        };
    }

    private static Course GetCourse()
    {
        return new Course()
        {
            Id = 1,
            Title = "Course 1",
            TeacherId = 1
        };
    }
}
