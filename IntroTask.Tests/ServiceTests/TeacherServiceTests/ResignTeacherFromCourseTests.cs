using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Service;
using Shared.Dtos.TeacherDtos;
using System.Linq.Expressions;

namespace IntroTask.Tests.ServiceTests.TeacherServiceTests;

public class ResignTeacherFromCourseTests
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

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldCallMapperOnce_IfValidInput(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), true);

        // Assert
        _mapperMock.Verify(m => m.Map(
            It.IsAny<TeacherUpdateDto>(),
            It.IsAny<Teacher>()),
            Times.Once);
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldCallGetTeacherByIdOnce_IfValidInput(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges);

        // Assert
        _repositoryMock.Verify(r =>
            r.Teacher.GetSingleOrDefaultAsync(
                AnyEntityPredicate<Teacher>(),
                AnyEntityInclude<Teacher>(),
                It.IsAny<bool>()),
            Times.Once);
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldCallGetCourseByIdOnce_IfValidInput(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges);

        // Assert
        _repositoryMock.Verify(r =>
            r.Course.GetSingleOrDefaultAsync(
                AnyEntityPredicate<Course>(),
                AnyEntityInclude<Course>(),
                It.IsAny<bool>()),
            Times.Once);
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldCallSaveChangesOnce_IfValidInput(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksPositiveScenario();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges);

        // Assert
        _repositoryMock.Verify(r =>
            r.SaveAsync(),
            Times.Once);
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldThrowException_IfTeacherIsNotFound(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksTeacherNotFound(id);

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<TeacherNotFoundException>(async () => await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges));
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldThrowException_IfCourseIsNotFound(
        int id,
        int courseId,
        bool trackChanges)
    {
        // Arrange
        SetupMocksCourseNotFound(courseId);

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<CourseNotFoundException>(async () => await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges));
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldThrowException_IfTeacherCourseNotConnected(
        int id,
        int courseId,
        bool trackChanges)
    {
        // Arrange
        SetupMocksNotConnectedEntities();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<TeacherCourseNotConnectedException>(async () => await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges));
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task ResignTeacherFromCourse_ShouldThrowException_IfSaveOperationFails(int id, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksSaveOperationFailed();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.ResignTeacherFromCourse(id, courseId, GetTeacherUpdateDto(), trackChanges));
    }

    private static TeacherUpdateDto GetTeacherUpdateDto()
    {
        return new()
        {
            Name = "John Smith"
        };
    }

    private void SetupMocksPositiveScenario()
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Teacher>(),
            AnyEntityInclude<Teacher>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetTeacherWithCourse());

        _repositoryMock.Setup(repo => repo.Course.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Course>(),
            AnyEntityInclude<Course>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetCourse(teacherId: 1));

        _mapperMock.Setup(m => m.Map(It.IsAny<TeacherUpdateDto>(),
            It.IsAny<Teacher>())).Returns(GetTeacher());

        _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
    }

    private void SetupMocksNotConnectedEntities()
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Teacher>(),
            AnyEntityInclude<Teacher>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetTeacherWithWrongCourse());

        _repositoryMock.Setup(repo => repo.Course.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Course>(),
            AnyEntityInclude<Course>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetCourse(teacherId: 2));
    }

    private void SetupMocksTeacherNotFound(int id)
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Teacher>(),
            AnyEntityInclude<Teacher>(),
            It.IsAny<bool>()))
                .ThrowsAsync(new TeacherNotFoundException(id));
    }

    private void SetupMocksCourseNotFound(int id)
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Teacher>(),
            AnyEntityInclude<Teacher>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetTeacherWithCourse());

        _repositoryMock.Setup(repo => repo.Course.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Course>(),
            AnyEntityInclude<Course>(),
            It.IsAny<bool>()))
                .ThrowsAsync(new CourseNotFoundException(id));
    }

    private void SetupMocksSaveOperationFailed()
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Teacher>(),
            AnyEntityInclude<Teacher>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetTeacherWithCourse());

        _repositoryMock.Setup(repo => repo.Course.GetSingleOrDefaultAsync(
            AnyEntityPredicate<Course>(),
            AnyEntityInclude<Course>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetCourse(teacherId: 1));

        _mapperMock.Setup(m => m.Map(It.IsAny<TeacherUpdateDto>(),
            It.IsAny<Teacher>())).Returns(GetTeacher());

        _repositoryMock.Setup(repo => repo.SaveAsync()).ThrowsAsync(new Exception());
    }

    private static Expression<Func<TEntity, bool>> AnyEntityPredicate<TEntity>()
    {
        return It.IsAny<Expression<Func<TEntity, bool>>>();
    }

    private static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> AnyEntityInclude<TEntity>()
    {
        return It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
    }

    private static Teacher GetTeacher()
    {
        return new Teacher
        {
            Id = 1,
            Name = "John Smith"
        };
    }

    private static Teacher GetTeacherWithCourse()
    {
        return new Teacher
        {
            Id = 1,
            Name = "John Smith",
            Courses = new List<Course>
            {
                new()
                {   
                    Id = 1,
                    Title = "Course 1",
                    TeacherId = 1,
                    Teacher = GetTeacher(),
                }
            }
        };
    }

    private static Teacher GetTeacherWithWrongCourse()
    {
        return new Teacher
        {
            Id = 1,
            Name = "John Smith",
            Courses = new List<Course>
            {
                new()
                {
                    Id = 2,
                    Title = "Course 2",
                    TeacherId = 2,
                }
            }
        };
    }

    private static Course GetCourse(int teacherId)
    {
        return new Course()
        {
            Id = 1,
            Title = "Course 1",
            TeacherId = teacherId
        };
    }
}
