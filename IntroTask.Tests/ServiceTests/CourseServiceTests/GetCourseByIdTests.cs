using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.CourseDtos;
using Shared.Dtos.StudentDtos;
using Shared.Dtos.TeacherDtos;
using System.Reflection;

namespace IntroTask.Tests.ServiceTests.CourseServiceTests;

public class GetCourseByIdTests
{
    private Mock<IRepositoryManager> _repositoryMock;
    private Mock<IMapper> _mapperMock;
    private CourseService? _sut;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
    }

    [TestCase(1, false)]
    [TestCase(1, true)]
    public async Task GetCourseByIdAsync_ShouldReturnResponseDtoWithCorrectId_IfIdExists(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();
        SetupMapperMockReturnsSigleDto();

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.GetCourseByIdAsync(id, trackChanges);

        // Assert
        Assert.That(result.Id, Is.EqualTo(id));
    }

    [TestCase(1, false)]
    [TestCase(1, true)]
    public async Task GetCourseByIdAsync_ShouldReturnCorrectType_IfIdExists(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();
        SetupMapperMockReturnsSigleDto();

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.GetCourseByIdAsync(id, trackChanges);

        // Assert
        Assert.That(result, Is.TypeOf<CourseResponseDto>());
    }

    [TestCase(3, false)]
    [TestCase(3, true)]
    public async Task GetCourseByIdAsync_ShouldThrowException_IfIdDoesNotExist(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockThrowsException(id);

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Assert
        var ex = Assert.ThrowsAsync<CourseNotFoundException>(async () =>
            await _sut.GetCourseByIdAsync(id, trackChanges));
    }

    private static CourseResponseDto GetCourseResponseDto()
    {
        return new CourseResponseDto(
            Id: 1,
            Title: "Course 1",
            Teacher: null,
            Students: new List<StudentShortResponseDto>()
            {
                new (1, "Jane", "Doe")
            });
    }

    private static Course GetCourse()
    {
        return new Course
        {
            Id = 1,
            Title = "Course 1",
            TeacherId = 1,
            Teacher = new Teacher { Id = 1, Name = "John Smith" },
            Students = new List<Student>
            {
                new()
                { Id = 1,
                    FirstName = "Jane",
                    LastName = "Doe"
                }
            }
        };
    }

    private void SetupMapperMockReturnsSigleDto()
    {
        _mapperMock.Setup(m =>
                    m.Map<CourseResponseDto>(
                    It.IsAny<Course>()))
                        .Returns(GetCourseResponseDto());
    }

    private void SetupRepositoryMockReturnsSingleEntity()
    {
        _repositoryMock.Setup(repo => repo.Course.GetCourseByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                        .ReturnsAsync(GetCourse());
    }

    private void SetupRepositoryMockThrowsException(int id)
    {
        _repositoryMock.Setup(repo => repo.Course.GetCourseByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                        .ThrowsAsync(new CourseNotFoundException(id));
    }
}
