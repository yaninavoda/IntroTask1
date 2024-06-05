using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.TeacherDtos;

namespace IntroTask.Tests.ServiceTests.TeacherServiceTests;

    public class GetTeacherByIdTests
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

    [TestCase(1, false)]
    [TestCase(1, true)]
    public async Task GetTeacherByIdAsync_ShouldReturnResponseDtoWithCorrectId_IfIdExists(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();
        SetupMapperMockReturnsSigleDto();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.GetTeacherByIdAsync(id, trackChanges);

        // Assert
        Assert.That(result.Id, Is.EqualTo(id));
    }

    [TestCase(1, false)]
    [TestCase(1, true)]
    public async Task GetTeacherByIdAsync_ShouldReturnCorrectType_IfIdExists(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();
        SetupMapperMockReturnsSigleDto();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.GetTeacherByIdAsync(id, trackChanges);

        // Assert
        Assert.That(result, Is.TypeOf<TeacherResponseDto>());
    }

    [TestCase(3, false)]
    [TestCase(3, true)]
    public async Task GetTeacherByIdAsync_ShouldThrowException_IfIdDoesNotExist(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockThrowsException(id);

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Assert
        var ex = Assert.ThrowsAsync<TeacherNotFoundException>(async () =>
            await _sut.GetTeacherByIdAsync(id, trackChanges));
    }

    private static TeacherResponseDto GetTeacherResponseDto()
    {
        return new(1, "John Smith", [new(1, "Course 1")]);
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

    private void SetupMapperMockReturnsSigleDto()
    {
        _mapperMock.Setup(m =>
                    m.Map<TeacherResponseDto>(
                    It.IsAny<Teacher>()))
                        .Returns(GetTeacherResponseDto());
    }

    private void SetupRepositoryMockReturnsSingleEntity()
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetTeacherByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                        .ReturnsAsync(GetTeacher());
    }

    private void SetupRepositoryMockThrowsException(int id)
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetTeacherByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                        .ThrowsAsync(new TeacherNotFoundException(id));
    }
}

