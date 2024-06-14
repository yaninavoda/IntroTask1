using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Service;
using System.Linq.Expressions;

namespace IntroTask.Tests.ServiceTests.StudentServiceTests;

public class DeleteStudentTests
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
    public async Task DeleteStudentAsync_ShouldBeCalledOnce_IfStudentIsFound(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.DeleteStudentAsync(id, trackChanges);

        // Assert
        _repositoryMock.Verify(
                repo =>
                repo.Student.Delete(It.IsAny<Student>()),
                Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task DeleteStudentAsync_ShouldCallSaveOnce_IfStudentIsFound(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.DeleteStudentAsync(id, trackChanges);

        // Assert
        _repositoryMock.Verify(
                repo =>
                repo.SaveAsync(),
                Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task DeleteStudentAsync_ShouldThrowException_IfStudentIsNotFound(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockThrowsException(id);


        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Assert
        var ex = Assert.ThrowsAsync<StudentNotFoundException>(async () =>
            await _sut.DeleteStudentAsync(id, trackChanges));
    }

    private void SetupRepositoryMockReturnsSingleEntity()
    {
        _repositoryMock.Setup(repo => repo.Student.GetSingleOrDefaultAsync(
                    AnyEntityPredicate<Student>(),
                    AnyEntityInclude<Student>(),
                    It.IsAny<bool>()))
                        .ReturnsAsync(GetStudent());

        _repositoryMock.Setup(repo => repo.Student.Delete(GetStudent())).Verifiable();

        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
    }

    private void SetupRepositoryMockThrowsException(int id)
    {
        _repositoryMock.Setup(repo => repo.Student.GetSingleOrDefaultAsync(
                    AnyEntityPredicate<Student>(),
                    AnyEntityInclude<Student>(),
                    It.IsAny<bool>()))
                        .ThrowsAsync(new StudentNotFoundException(id));
    }

    private static Student GetStudent()
    {
        return new()
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

    private static Expression<Func<TEntity, bool>> AnyEntityPredicate<TEntity>()
    {
        return It.IsAny<Expression<Func<TEntity, bool>>>();
    }

    private static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> AnyEntityInclude<TEntity>()
    {
        return It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
    }
}
