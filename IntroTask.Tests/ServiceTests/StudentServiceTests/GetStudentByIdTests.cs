using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.StudentDtos;


namespace IntroTask.Tests.ServiceTests.StudentServiceTests;

public class GetStudentByIdTests
{
    private Mock<IRepositoryManager> _repositoryMock;
    private Mock<IMapper> _mapperMock;
    private StudentService? _sut;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
    }

    [TestCase(1, false)]
    [TestCase(1, true)]
    public async Task GetStudentByIdAsync_ShouldReturnResponseDtoWithCorrectId_IfIdExists(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();
        SetupMapperMockReturnsSigleDto();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.GetStudentByIdAsync(id, trackChanges);

        // Assert
        Assert.That(result.Id, Is.EqualTo(id));
    }

    [TestCase(1, false)]
    [TestCase(1, true)]
    public async Task GetStudentByIdAsync_ShouldReturnCorrectType_IfIdExists(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();
        SetupMapperMockReturnsSigleDto();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.GetStudentByIdAsync(id, trackChanges);

        // Assert
        Assert.That(result, Is.TypeOf<StudentResponseDto>());
    }

    [TestCase(3, false)]
    [TestCase(3, true)]
    public async Task GetStudentByIdAsync_ShouldThrowException_IfIdDoesNotExist(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockThrowsException(id);

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Assert
        var ex = Assert.ThrowsAsync<StudentNotFoundException>(async () =>
            await _sut.GetStudentByIdAsync(id, trackChanges));
    }

    private static StudentResponseDto GetStudentResponseDto()
    {
        return new(1, "Jane", "Doe", [new(1, "Course 1")]);
    }

    private static Student GetStudent()
    {
        return new Student
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
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
                    m.Map<StudentResponseDto>(
                    It.IsAny<Student>()))
                        .Returns(GetStudentResponseDto());
    }

    private void SetupRepositoryMockReturnsSingleEntity()
    {
        _repositoryMock.Setup(repo => repo.Student.GetStudentByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                        .ReturnsAsync(GetStudent());
    }

    private void SetupRepositoryMockThrowsException(int id)
    {
        _repositoryMock.Setup(repo => repo.Student.GetStudentByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                        .ThrowsAsync(new StudentNotFoundException(id));
    }
}
