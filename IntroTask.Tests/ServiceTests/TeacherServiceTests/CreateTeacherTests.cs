using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.TeacherDtos;

namespace IntroTask.Tests.ServiceTests.TeacherServiceTests;

public class CreateTeacherTests
{
    private Mock<IRepositoryManager> _repositoryMock;
    private Mock<IMapper> _mapperMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
    }

    [Test]
    public async Task CreateTeacherAsync_ShouldReturnCorrectType_IfSuppliedCorrectInput()
    {
        // Arrange
        SetupMockPositiveScenario();

        var _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.CreateTeacherAsync(GetTeacherCreateDto());

        // Assert
        Assert.That(result, Is.TypeOf<TeacherShortResponseDto>());
    }

    private void SetupMockPositiveScenario()
    {
        _mapperMock.Setup(m => m.Map<Teacher>(It.IsAny<TeacherCreateDto>()))
                    .Returns(GetTeacher());

        _repositoryMock.Setup(repo => repo.Teacher.CreateTeacher(GetTeacher())).Verifiable();
        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

        _mapperMock.Setup(m => m.Map<TeacherShortResponseDto>(It.IsAny<Teacher>()))
            .Returns(GetTeacherShortResponseDto());
    }

    [Test]
    public async Task CreateTeacherAsync_ShouldReturnCorrectDto_IfSuppliedCorrectInput()
    {
        // Arrange
        var expected = GetTeacherShortResponseDto();

        SetupMockPositiveScenario();

        var _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.CreateTeacherAsync(GetTeacherCreateDto());

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task CreateTeacherAsync_ShouldThrowExceptionOnSaveFailure()
    {
        SetupMockPositiveScenario();
        _repositoryMock.Setup(repo => repo.SaveAsync()).ThrowsAsync(new Exception());

        var _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert  
        Assert.ThrowsAsync<Exception>(async () => await _sut.CreateTeacherAsync(GetTeacherCreateDto()));
    }

    private static TeacherShortResponseDto GetTeacherShortResponseDto()
    {
        return new(1, "John Smith");
    }

    private static TeacherCreateDto GetTeacherCreateDto()
    {
        return new () { Name = "John Smith" };
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
}
