using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Service;
using Shared.Dtos.StudentDtos;
using System.Linq.Expressions;

namespace IntroTask.Tests.ServiceTests.StudentServiceTests;

public class UpdateStudentTests
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

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task UpdateStudentAsync_ShouldCallMapOnce_IfValidInput(int id, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.UpdateStudentAsync(id, GetStudentUpdateDto(), trackChanges);

        // Assert
        _mapperMock.Verify(m => m.Map(
            It.IsAny<StudentUpdateDto>(),
            It.IsAny<Student>()),
            Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task UpdateStudentAsync_ShouldCallGetStudentByIdOnce_IfValidInput(int id, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.UpdateStudentAsync(id, GetStudentUpdateDto(), trackChanges);

        // Assert
        _repositoryMock.Verify(r =>
            r.Student.GetSingleOrDefaultAsync(
                AnyEntityPredicate<Student>(),
                AnyEntityInclude<Student>(),
                It.IsAny<bool>()),
            Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task UpdateStudentAsync_ShouldThrowException_IfSaveOperationFails(int id, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();
        _repositoryMock.Setup(repo => repo.SaveAsync()).ThrowsAsync(new Exception());

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.UpdateStudentAsync(id, GetStudentUpdateDto(), trackChanges));
    }


    private static StudentUpdateDto GetStudentUpdateDto()
    {
        return new()
        {
            FirstName = "Jane",
            LastName = "Doe"
        };
    }

    private void SetupMocksPositiveScenario()
    {
        _repositoryMock.Setup(repo => repo.Student.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Student>(),
            AnyEntityInclude<Student>(),
            It.IsAny<bool>()))
                        .ReturnsAsync(GetStudent());

        _mapperMock.Setup(m => m.Map(It.IsAny<StudentUpdateDto>(),
            It.IsAny<Student>())).Returns(GetStudent());

        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
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

    private static Expression<Func<TEntity, bool>> AnyEntityPredicate<TEntity>()
    {
        return It.IsAny<Expression<Func<TEntity, bool>>>();
    }

    private static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> AnyEntityInclude<TEntity>()
    {
        return It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
    }
}
