using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Service;
using Shared.Dtos.StudentDtos;
using System.Linq.Expressions;

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

    [Test]
    public async Task GetAllStudentsAsync_ShouldReturnStudentShortResponseDtos_IfStudentsExist()
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var studentDtos = await _sut.GetAllStudentsAsync();

        // Assert
        Assert.That(studentDtos, Is.InstanceOf<IEnumerable<StudentShortResponseDto>>());
    }

    [Test]
    public async Task GetAllStudentsAsync_ShouldReturnCorrectAmmountOfDtos_IfStudentsExist()
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var studentDtos = await _sut.GetAllStudentsAsync();

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
        var studentDtos = await _sut.GetAllStudentsAsync();

        // Assert
        Assert.That(studentDtos, Is.Empty);
    }

    [Test]
    public async Task GetAllStudentsAsync_ShouldBeCalledOnce_IfStudentsExist()
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var studentDtos = await _sut.GetAllStudentsAsync();

        // Assert
        _repositoryMock.Verify(repo => repo.Student.GetAllAsync(null, null), Times.Once);
    }

    [Test]
    public async Task GetAllStudentsAsync_ShouldReturnEmptyList_IfNoStudentsFound()
    {
        // Arrange
        SetupRepositoryMockReturnsEmptyCollection();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var studentDtos = await _sut.GetAllStudentsAsync();

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
        _repositoryMock.Setup(repo => repo.Student.GetAllAsync(
            AnyEntityPredicate<Student>(),
            AnyEntityInclude<Student>()))
            .ReturnsAsync(GetStudents());
    }

    private void SetupRepositoryMockReturnsEmptyCollection()
    {
        _repositoryMock.Setup(repo => repo.Student.GetAllAsync(
            AnyEntityPredicate<Student>(),
            AnyEntityInclude<Student>()))
                .ReturnsAsync(new List<Student>());
    }

    private static Expression<Func<TEntity, bool>> AnyEntityPredicate<TEntity>()
    {
        return It.IsAny<Expression<Func<TEntity, bool>>>();
    }

    private static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> AnyEntityInclude<TEntity>()
    {
        return It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
    }
}
