using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Moq;
using NUnit.Framework.Internal;
using Service;
using Shared.Dtos.TeacherDtos;

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

    [TestCase(true)]
    [TestCase(false)]
    public async Task GetAllTeachersAsync_ShouldReturnTeacherShortResponseDtos_IfTeachersExist(bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync(trackChanges);

        // Assert
        Assert.That(teacherDtos, Is.InstanceOf<IEnumerable<TeacherShortResponseDto>>());
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task GetAllTeachersAsync_ShouldReturnCorrectAmmountOfDtos_IfTeachersExist(bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync(trackChanges);

        // Assert
        Assert.That(teacherDtos.Count, Is.EqualTo(GetTeachers().Count()));
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task GetAllTeachersAsync_ShouldBeCalledOnce_IfTeacherExist(bool trackChanges)
    {
        // Arrange
        SetupRepositoryMockReturnsDataCollection();
        SetupMapperMockReturnsDataCollection();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync(trackChanges);

        // Assert
        _repositoryMock.Verify(repo => repo.Teacher.GetAllTeachersAsync(trackChanges), Times.Once);
    }


    [Test]
    public async Task GetAllTeachersAsync_ShouldReturnEmptyList_IfNoTeachersFound()
    {
        // Arrange
        SetupRepositoryMockReturnsEmptyCollection();

        _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync(trackChanges: false);

        // Assert
        Assert.That(teacherDtos, Is.Empty);
    }

    private static TeacherResponseDto GetTeacherResponseDto()
    {
        return new(1, "John Smith", [new(1, "Course 1")]);
    }

    private static Teacher GetTeacher()
    {
        return new Teacher
        {
            Id = 1,
            Name = "John Smith",
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
        _repositoryMock.Setup(repo => repo.Teacher.GetAllTeachersAsync(It.IsAny<bool>()))
            .ReturnsAsync(GetTeachers());
    }

    private void SetupRepositoryMockReturnsEmptyCollection()
    {
        _repositoryMock.Setup(repo => repo.Teacher.GetAllTeachersAsync(It.IsAny<bool>()))
                .ReturnsAsync(new List<Teacher>());
    }

    
}
