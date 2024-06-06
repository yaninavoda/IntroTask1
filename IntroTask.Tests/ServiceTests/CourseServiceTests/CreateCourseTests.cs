using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.CourseDtos;

namespace IntroTask.Tests.ServiceTests.CourseServiceTests;

public class CreateCourseTests
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

    [Test]
    public async Task CreateCourseAsync_ShouldReturnCorrectType_IfSuppliedCorrectInput()
    {
        // Arrange
        SetupMockPositiveScenario();

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.CreateCourseAsync(GetCourseCreateDto());

        // Assert
        Assert.That(result, Is.TypeOf<CourseShortResponseDto>());
    }

    [Test]
    public async Task CreateCourseAsync_ShouldReturnCorrectDto_IfSuppliedCorrectInput()
    {
        // Arrange
        var expected = GetCourseShortResponseDto();

        SetupMockPositiveScenario();

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.CreateCourseAsync(GetCourseCreateDto());

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task CreateCourseAsync_ShouldThrowExceptionOnSaveFailure()
    {
        // Arrange
        SetupMocksSaveOperationFailed();

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert  
        Assert.ThrowsAsync<Exception>(async () => await _sut.CreateCourseAsync(GetCourseCreateDto()));
    }

    private static CourseShortResponseDto GetCourseShortResponseDto()
    {
        return new(1, "Course 1");
    }

    private static CourseCreateDto GetCourseCreateDto()
    {
        return new() { Title = "Course 1" };
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

    private void SetupMockPositiveScenario()
    {
        _mapperMock.Setup(m => m.Map<Course>(It.IsAny<CourseCreateDto>()))
                    .Returns(GetCourse());

        _repositoryMock.Setup(repo => repo.Course.CreateCourse(GetCourse())).Verifiable();
        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

        _mapperMock.Setup(m => m.Map<CourseShortResponseDto>(It.IsAny<Course>()))
            .Returns(GetCourseShortResponseDto());
    }

    private void SetupMocksSaveOperationFailed()
    {
        _mapperMock.Setup(m => m.Map<Course>(It.IsAny<CourseCreateDto>()))
                    .Returns(GetCourse());

        _repositoryMock.Setup(repo => repo.Course.CreateCourse(GetCourse())).Verifiable();
        _repositoryMock.Setup(repo => repo.SaveAsync()).ThrowsAsync(new Exception());


        _mapperMock.Setup(m => m.Map<CourseShortResponseDto>(It.IsAny<Course>()))
            .Returns(GetCourseShortResponseDto());
    }
}
