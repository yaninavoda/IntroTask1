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
    private TeacherService _sut;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldCallMapperOnce_IfValidInput(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), true);

        // Assert
        _mapperMock.Verify(m => m.Map(
            It.IsAny<TeacherUpdateDto>(),
            It.IsAny<Teacher>()),
            Times.Once);
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldCallGetTeacherByIdOnce_IfValidInput(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges);

        // Assert
        _repositoryMock.Verify(r =>
            r.Teacher.GetTeacherByIdAsync(
            It.IsAny<int>(),
            It.IsAny<bool>()),
            Times.Once);
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldCallGetCourseByIdOnce_IfValidInput(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges);

        // Assert
        _repositoryMock.Verify(r =>
            r.Course.GetCourseByIdAsync(
            It.IsAny<int>(),
            It.IsAny<bool>()),
            Times.Once);
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldCallSaveChangesOnce_IfValidInput(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges);

        // Assert
        _repositoryMock.Verify(r =>
            r.SaveAsync(),
            Times.Once);
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task UpdateTeacherAsync_ShouldThrowException_IfSaveOperationFails(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksSaveOperationFailed();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges));
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
                .ReturnsAsync(GetTeacherWithCourse());

        _repositoryMock.Setup(repo => repo.Course.GetCourseByIdAsync(
            It.IsAny<int>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetCourse());

        _mapperMock.Setup(m => m.Map(It.IsAny<TeacherUpdateDto>(),
            It.IsAny<Teacher>())).Returns(GetTeacher());

        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
    }

    private void SetupMocksSaveOperationFailed()
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetTeacherByIdAsync(
            It.IsAny<int>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetTeacherWithCourse());

        _repositoryMock.Setup(repo => repo.Course.GetCourseByIdAsync(
            It.IsAny<int>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetCourse());

        _mapperMock.Setup(m => m.Map(It.IsAny<TeacherUpdateDto>(),
            It.IsAny<Teacher>())).Returns(GetTeacher());

        _repositoryMock.Setup(repo => repo.SaveAsync()).ThrowsAsync(new Exception());
    }

    private static Teacher GetTeacher()
    {
        return new Teacher
        {
            Id = 1,
            Name = "John Smith"
        };
    }

    private static Teacher GetTeacherWithCourse()
    {
        return new Teacher
        {
            Id = 1,
            Name = "John Smith",
            Courses = new List<Course>
            {
                new()
                {   
                    Id = 1,
                    Title = "Course 1",
                    TeacherId = 1,
                    Teacher = GetTeacher(),
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
