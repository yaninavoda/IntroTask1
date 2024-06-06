using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.StudentDtos;

namespace IntroTask.Tests.ServiceTests.StudentServiceTests;

public class GetAllStudentsTests
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

    [TestCase(true)]
    [TestCase(false)]
    public async Task GetAllStudentsAsync_ShouldReturnStudentShortResponseDtos_IfStudentsExist(bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var studentDtos = await _sut.GetAllStudentsAsync(trackChanges);

        // Assert
        Assert.That(studentDtos, Is.InstanceOf<IEnumerable<StudentShortResponseDto>>());
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task GetAllStudentsAsync_ShouldReturnCorrectAmmountOfDtos_IfStudentsExist(bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var studentDtos = await _sut.GetAllStudentsAsync(trackChanges);

        // Assert
        Assert.That(studentDtos.Count, Is.EqualTo(GetStudents().Count()));
    }



    [Test]
    public async Task GetAllCoursesAsync_ShouldReturnEmptyList_IfNoCoursesFound()
    {
        // Arrange
        SetupRepositoryMockReturnsEmptyCollection();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var studentDtos = await _sut.GetAllStudentsAsync(trackChanges: false);

        // Assert
        Assert.That(studentDtos, Is.Empty);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task GetAllStudentsAsync_ShouldBeCalledOnce_IfStudentsExist(bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var studentDtos = await _sut.GetAllStudentsAsync(trackChanges);

        // Assert
        _repositoryMock.Verify(repo => repo.Student.GetAllStudentsAsync(trackChanges), Times.Once);
    }

    [Test]
    public async Task GetAllStudentsAsync_ShouldReturnEmptyList_IfNoStudentsFound()
    {
        // Arrange
        SetupRepositoryMockReturnsEmptyCollection();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var studentDtos = await _sut.GetAllStudentsAsync(trackChanges: false);

        // Assert
        Assert.That(studentDtos, Is.Empty);
    }

    private static List<StudentShortResponseDto> GetStudentShortResponseDtos()
    {
        var dtos = new List<StudentShortResponseDto>
        {
            new (1, "Jane", "Doe"),
            new (2, "John", "Baker"),
        };

        return dtos;
    }

    private static List<Student> GetStudents()
    {
        var students = new List<Student>
        {
            new ()
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe"
            },
            new ()
            {
                Id = 2,
                FirstName = "John",
                LastName = "Baker"
            },
        };

        return students;
    }

    private void SetupMapperMockReturnsDataCollection()
    {
        _mapperMock.Setup(m => m.Map<List<StudentShortResponseDto>>(It.IsAny<IEnumerable<Student>>()))
                        .Returns(GetStudentShortResponseDtos());
    }

    private void SetupRepositoryMockReturnsDataCollection()
    {
        _repositoryMock.Setup(repo => repo.Student.GetAllStudentsAsync(It.IsAny<bool>()))
            .ReturnsAsync(GetStudents());
    }

    private void SetupRepositoryMockReturnsEmptyCollection()
    {
        _repositoryMock.Setup(repo => repo.Student.GetAllStudentsAsync(It.IsAny<bool>()))
                .ReturnsAsync(new List<Student>());
    }
}
