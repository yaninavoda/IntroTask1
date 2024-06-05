using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.TeacherDtos;

namespace IntroTask.Tests.ServiceTests.TeacherServiceTests;

public class UpdateTeacherTests
{
    private Mock<IRepositoryManager> _repositoryMock;
    private Mock<IMapper> _mapperMock;
    private TeacherService? _sut;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task UpdateTeacherAsync_ShouldCallMapOnce_IfValidInput(int id, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.UpdateTeacherAsync(id, GetTeacherUpdateDto(), trackChanges);

        // Assert
        _mapperMock.Verify(m => m.Map(
            It.IsAny<TeacherUpdateDto>(),
            It.IsAny<Teacher>()),
            Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task UpdateTeacherAsync_ShouldCallGetTeacherByIdOnce_IfValidInput(int id, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.UpdateTeacherAsync(id, GetTeacherUpdateDto(), trackChanges);

        // Assert
        _repositoryMock.Verify(r => 
            r.Teacher.GetTeacherByIdAsync(
            It.IsAny<int>(),
            It.IsAny<bool>()),
            Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task UpdateTeacherAsync_ShouldThrowException_IfSaveOperationFails(int id, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();
        _repositoryMock.Setup(repo => repo.SaveAsync()).ThrowsAsync(new Exception());

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.UpdateTeacherAsync(id, GetTeacherUpdateDto(), trackChanges));
    }


    private static TeacherUpdateDto GetTeacherUpdateDto()
    {
        return new ()
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
}
