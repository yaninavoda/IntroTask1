using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Service;
using Shared.Dtos.CourseDtos;
using System.Linq.Expressions;

namespace IntroTask.Tests.ServiceTests.CourseServiceTests;

public class UpdateCourseTests
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
    public async Task UpdateTeacherAsync_ShouldCallMapOnce_IfValidInput(int id, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.UpdateCourseAsync(id, GetCourseUpdateDto(), trackChanges);

        // Assert
        _mapperMock.Verify(m => m.Map(
            It.IsAny<CourseUpdateDto>(),
            It.IsAny<Course>()),
            Times.Once);
    }

    [TestCase(1, true)]
    [TestCase(1, false)]
    public async Task UpdateTeacherAsync_ShouldCallGetTeacherByIdOnce_IfValidInput(int id, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.UpdateCourseAsync(id, GetCourseUpdateDto(), trackChanges);

        // Assert
        _repositoryMock.Verify(r =>
            r.Course.GetSingleOrDefaultAsync(
                AnyEntityPredicate<Course>(),
                AnyEntityInclude<Course>(),
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

        _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.UpdateCourseAsync(id, GetCourseUpdateDto(), trackChanges));
    }


    private static CourseUpdateDto GetCourseUpdateDto()
    {
        return new()
        {
            Title = "Course 1"
        };
    }

    private void SetupMocksPositiveScenario()
    {
        _repositoryMock.Setup(repo => repo.Course.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Course>(),
            AnyEntityInclude<Course>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetCourse());

        _mapperMock.Setup(m => m.Map(It.IsAny<CourseUpdateDto>(),
            It.IsAny<Course>())).Returns(GetCourse());

        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
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

    private static Expression<Func<TEntity, bool>> AnyEntityPredicate<TEntity>()
    {
        return It.IsAny<Expression<Func<TEntity, bool>>>();
    }

    private static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> AnyEntityInclude<TEntity>()
    {
        return It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
    }
}
