using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Service;
using System.Linq.Expressions;

namespace IntroTask.Tests.ServiceTests.CourseServiceTests;

public class DeleteCourseTests
{
    private Mock<IRepositoryManager> _repositoryMock;
    private Mock<IMapper> _mapperMock;
    private CourseService? _sut;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task DeleteCourseAsync_ShouldBeCalledOnce_IfCourseIsFound(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.DeleteCourseAsync(id, trackChanges);

        // Assert
        _repositoryMock.Verify(
                repo =>
                repo.Course.Delete(It.IsAny<Course>()),
                Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task DeleteCourseAsync_ShouldCallSaveOnce_IfCourseIsFound(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsSingleEntity();

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.DeleteCourseAsync(id, trackChanges);

        // Assert
        _repositoryMock.Verify(
                repo =>
                repo.SaveAsync(),
                Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task DeleteCourseAsync_ShouldThrowException_IfCourseIsNotFound(int id, bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockThrowsException(id);


        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Assert
        var ex = Assert.ThrowsAsync<CourseNotFoundException>(async () =>
            await _sut.DeleteCourseAsync(id, trackChanges));
    }
    private static Course GetCourse()
    {
        return new Course
        {
            Id = 1,
            Title = "Course 1",
            TeacherId = 1,
            Teacher = new Teacher { Id = 1, Name = "John Smith" },
            Students = new List<Student>
            {
                new()
                { Id = 1,
                    FirstName = "Jane",
                    LastName = "Doe"
                }
            }
        };
    }

    private void SetupRepositoryMockReturnsSingleEntity()
    {
        _repositoryMock.Setup(repo => repo.Course.GetSingleOrDefaultAsync(
                    AnyEntityPredicate<Course>(),
                    AnyEntityInclude<Course>(),
                    It.IsAny<bool>()))
                        .ReturnsAsync(GetCourse());

        _repositoryMock.Setup(repo => repo.Course.Delete(GetCourse())).Verifiable();

        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
    }

    private void SetupRepositoryMockThrowsException(int id)
    {
        _repositoryMock.Setup(repo => repo.Course.GetSingleOrDefaultAsync(
                    AnyEntityPredicate<Course>(),
                    AnyEntityInclude<Course>(),
                    It.IsAny<bool>()))
                        .ThrowsAsync(new CourseNotFoundException(id));
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
