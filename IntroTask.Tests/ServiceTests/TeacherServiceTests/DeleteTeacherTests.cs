using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Moq;
using Service;

namespace IntroTask.Tests.ServiceTests.TeacherServiceTests;

public class DeleteTeacherTests
{
    private Mock<IRepositoryManager> _repositoryMock;
    private Mock<IMapper> _mapperMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task DeleteTeacherAsync_ShouldBeCalledOnce_IfTeacherIsFound(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();

        var _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.DeleteTeacherAsync(id, trackChanges);

        // Assert
        _repositoryMock.Verify(
                repo =>
                repo.Teacher.DeleteTeacher(It.IsAny<Teacher>()),
                Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task DeleteTeacherAsync_ShouldCallSaveOnce_IfTeacherIsFound(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();

        var _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.DeleteTeacherAsync(id, trackChanges);

        // Assert
        _repositoryMock.Verify(
                repo =>
                repo.SaveAsync(),
                Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task DeleteTeacherAsync_ShouldThrowException_IfTeacherIsNotFound(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockThrowsException(id);

        var _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Assert
        var ex = Assert.ThrowsAsync<TeacherNotFoundException>(async () =>
            await _sut.DeleteTeacherAsync(id, trackChanges));
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

    private void SetupRepositoryMockReturnsSingleEntity()
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetTeacherByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                        .ReturnsAsync(GetTeacher());

        _repositoryMock.Setup(repo => repo.Teacher.DeleteTeacher(GetTeacher())).Verifiable();

        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
    }

    private void SetupRepositoryMockThrowsException(int id)
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetTeacherByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                        .ThrowsAsync(new TeacherNotFoundException(id));
    }
}
