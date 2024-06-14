using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.StudentDtos;

namespace IntroTask.Tests.ServiceTests.StudentServiceTests;

public class CreateStudentTests
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

    [Test]
    public async Task CreateStudentAsync_ShouldReturnCorrectType_IfSuppliedCorrectInput()
    {
        // Arrange
        SetupMockPositiveScenario();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.CreateStudentAsync(GetStudentCreateDto());

        // Assert
        Assert.That(result, Is.TypeOf<StudentShortResponseDto>());
    }

    [Test]
    public async Task CreateStudentAsync_ShouldReturnCorrectDto_IfSuppliedCorrectInput()
    {
        // Arrange
        var expected = GetStudentShortResponseDto();

        SetupMockPositiveScenario();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var result = await _sut.CreateStudentAsync(GetStudentCreateDto());

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task
        CreateStudentAsync_ShouldThrowExceptionOnSaveFailure()
    {
        // Arrange
        SetupMocksSaveOperationFailed();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert  
        Assert.ThrowsAsync<Exception>(async () => await _sut.CreateStudentAsync(GetStudentCreateDto()));
    }

    private void SetupMocksSaveOperationFailed()
    {
        _mapperMock.Setup(m => m.Map<Student>(It.IsAny<StudentCreateDto>()))
                    .Returns(GetStudent());

        _repositoryMock.Setup(repo => repo.Student.Create(GetStudent())).Verifiable();
        _repositoryMock.Setup(repo => repo.SaveAsync()).ThrowsAsync(new Exception());


        _mapperMock.Setup(m => m.Map<StudentShortResponseDto>(It.IsAny<Student>()))
            .Returns(GetStudentShortResponseDto());
    }

    private void SetupMockPositiveScenario()
    {
        _mapperMock.Setup(m => m.Map<Student>(It.IsAny<StudentCreateDto>()))
                    .Returns(GetStudent());

        _repositoryMock.Setup(repo => repo.Student.Create(GetStudent())).Verifiable();
        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

        _mapperMock.Setup(m => m.Map<StudentShortResponseDto>(It.IsAny<Student>()))
            .Returns(GetStudentShortResponseDto());
    }

    private static StudentShortResponseDto GetStudentShortResponseDto()
    {
        return new StudentShortResponseDto(1, "Jane", "Doe");
    }

    private static StudentCreateDto GetStudentCreateDto()
    {
        return new StudentCreateDto
        {
            FirstName = "Jane",
            LastName = "Doe"
        };
    }

    private static Student GetStudent()
    {
        return new ()
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Courses = new List<Course>
            {
                new ()
                {   Id = 1,
                    Title = "Course 1",
                    TeacherId = 1
                }
            }
        };
    }
}
