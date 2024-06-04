using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.TeacherDtos;

namespace IntroTask.Tests.ServiceTests;

public class TeacherServiceTests
{
    private Mock<IRepositoryManager> _repositoryMock;
    private Mock<IMapper> _mapperMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task GetAllTeachersAsync_ShouldReturnTeacherShortResponseDtos_IrrespectiveToTrackChanges(bool trackChanges)
    {
        // Arrange
        var teachers = GetTeachers();
        _repositoryMock.Setup(repo => repo.Teacher.GetAllTeachersAsync(It.IsAny<bool>())).ReturnsAsync(GetTeachers());
        _mapperMock.Setup(m => m.Map<List<TeacherShortResponseDto>>(It.IsAny<IEnumerable<Teacher>>()))
                .Returns(GetTeacherShortResponseDtos());

        var _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

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

        var teachers = GetTeachers();
        var teacherResponseDtos = GetTeacherShortResponseDtos();

        _repositoryMock.Setup(repo => repo.Teacher.GetAllTeachersAsync(It.IsAny<bool>())).ReturnsAsync(teachers);
        _mapperMock.Setup(m => m.Map<List<TeacherShortResponseDto>>(It.IsAny<IEnumerable<Teacher>>()))
                .Returns(teacherResponseDtos);

        var _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync(trackChanges);

        // Assert
        Assert.That(teacherDtos.Count, Is.EqualTo(teachers.Count()));
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task GetAllTeachersAsync_ShouldBeCalledOnce(bool trackChanges)
    {
        // Arrange

        var teachers = GetTeachers();
        var teacherResponseDtos = GetTeacherShortResponseDtos();

        _repositoryMock.Setup(repo => repo.Teacher.GetAllTeachersAsync(It.IsAny<bool>())).ReturnsAsync(teachers);
        _mapperMock.Setup(m => m.Map<List<TeacherShortResponseDto>>(It.IsAny<IEnumerable<Teacher>>()))
                .Returns(teacherResponseDtos);

        var _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync(trackChanges);

        // Assert
        _repositoryMock.Verify(repo => repo.Teacher.GetAllTeachersAsync(trackChanges), Times.Once);
    }


    [Test]
    [Category("Empty List")]
    public async Task GetAllTeachersAsync_ShouldReturnEmptyList_IfNoTeachersFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.Teacher.GetAllTeachersAsync(It.IsAny<bool>()))
        .ReturnsAsync(new List<Teacher>());

        var _sut = new TeacherService(_repositoryMock.Object, _mapperMock.Object);

        // Act
        var teacherDtos = await _sut.GetAllTeachersAsync(trackChanges: false);

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
}
