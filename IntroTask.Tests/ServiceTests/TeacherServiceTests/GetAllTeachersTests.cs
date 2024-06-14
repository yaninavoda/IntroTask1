using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework.Internal;
using Service;
using Shared.Dtos.TeacherDtos;
using System.Linq.Expressions;

namespace IntroTask.Tests.ServiceTests.TeacherServiceTests;

public class GetAllTeachersTests
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

    [Test]
    public async Task GetAllTeachersAsync_ShouldReturnTeacherShortResponseDtos_IfTeachersExist()
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync();

        // Assert
        Assert.That(teacherDtos, Is.InstanceOf<IEnumerable<TeacherShortResponseDto>>());
    }

    [Test]
    public async Task GetAllTeachersAsync_ShouldReturnCorrectAmmountOfDtos_IfTeachersExist()
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync();

        // Assert
        Assert.That(teacherDtos.Count, Is.EqualTo(GetTeachers().Count()));
    }

    [Test]
    public async Task GetAllTeachersAsync_ShouldBeCalledOnce_IfTeacherExist()
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync();

        // Assert
        _repositoryMock.Verify(repo => repo.Teacher.GetAllAsync(null, null), Times.Once);
    }


    [Test]
    public async Task GetAllTeachersAsync_ShouldReturnEmptyList_IfNoTeachersFound()
    {
        // Arrange
        SetupRepositoryMockReturnsEmptyCollection();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync();

        // Assert
        Assert.That(teacherDtos, Is.Empty);
    }

    private static List<TeacherShortResponseDto> GetTeacherShortResponseDtos()
    {
        var dtos = new List<TeacherShortResponseDto>
        {
            new (Id: 1, Name: "John Smith"),
            new (Id: 2, Name: "John Baker"),
        };

        return dtos;
    }

    private static List<Teacher> GetTeachers()
    {
        var teachers = new List<Teacher>
        {
            new ()
            {
                Id = 1,
                Name = "John Smith"
            },
            new ()
            {
                Id = 2,
                Name = "John Baker"
            },
        };

        return teachers;
    }

    private void SetupMapperMockReturnsDataCollection()
    {
        _mapperMock.Setup(m => m.Map<List<TeacherShortResponseDto>>(It.IsAny<IEnumerable<Teacher>>()))
                        .Returns(GetTeacherShortResponseDtos());
    }

    private void SetupRepositoryMockReturnsDataCollection()
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetAllAsync(
            AnyEntityPredicate<Teacher>(),
            AnyEntityInclude<Teacher>()))
                .ReturnsAsync(GetTeachers());
    }

    private void SetupRepositoryMockReturnsEmptyCollection()
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetAllAsync(
            AnyEntityPredicate<Teacher>(),
            AnyEntityInclude<Teacher>()))
                .ReturnsAsync(new List<Teacher>());
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
