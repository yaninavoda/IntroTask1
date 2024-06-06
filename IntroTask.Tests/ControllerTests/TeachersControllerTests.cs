using Entities.Exceptions;
using IntroTask.Entities;
using IntroTaskWebApi.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Contracts;
using Shared.Dtos.TeacherDtos;

namespace IntroTask.Tests.ControllerTests;

public class TeachersControllerTests
{
    private Mock<IServiceManager> _mockServiceManager;
    private Mock<ITeacherService> _mockTeacherService;
    private TeachersController _sut;

    [SetUp]
    public void Setup()
    {
        _mockServiceManager = new Mock<IServiceManager>();
        _mockTeacherService = new Mock<ITeacherService>();
    }

    [Test]
    public async Task GetTeachers_ShouldReturnOkObjectResult()
    {
        // Arrange

        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);
        _mockTeacherService.Setup(s => s.GetAllTeachersAsync(false))
            .ReturnsAsync(GetTeacherShortResponseDtos(2));

        _sut = new TeachersController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetTeachers();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [TestCase(2)]
    [TestCase(0)]
    public async Task GetTeachers_ShouldReturnListOfTeachers(int countToReturn)
    {
        // Arrange

        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);
        _mockTeacherService.Setup(s => s.GetAllTeachersAsync(false))
            .ReturnsAsync(GetTeacherShortResponseDtos(countToReturn));

        _sut = new TeachersController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetTeachers();
        var okObjectResult = (OkObjectResult)result;

        // Assert
        Assert.That(okObjectResult.Value, Is.TypeOf<List<TeacherShortResponseDto>>());
    }
    [TestCase(1)]
    public async Task GetTeacher_ShouldReturnOkObjectResult_WhenTeacherIsFound(int teacherId)
    {
        // Arrange
        var expected = GetTeacherResponseDto();

        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);
        _mockTeacherService.Setup(s => s.GetTeacherByIdAsync(teacherId, false))
            .ReturnsAsync(GetTeacherResponseDto());

        _sut = new TeachersController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetTeacher(teacherId);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }


    [TestCase(1)]
    public async Task GetTeacher_ShouldReturnTeacher_WhenTeacherIsFound(int teacherId)
    {
        // Arrange
        var expected = GetTeacherResponseDto();

        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);
        _mockTeacherService.Setup(s => s.GetTeacherByIdAsync(teacherId, false))
            .ReturnsAsync(GetTeacherResponseDto());

        _sut = new TeachersController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetTeacher(teacherId);
        var okObjectResult = (OkObjectResult)result;

        // Assert
        Assert.That(okObjectResult.Value, Is.EqualTo(expected));
    }

    [Test]
    public async Task CreateTeacher_ShouldReturnCreatedAtRoute_OnSuccess()
    {
        // Arrange
        var teacherCreateDto = GetTeacherCreateDto();

        var createdTeacher = GetTeacherShortResponseDto();

        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);
        _mockTeacherService.Setup(s => s.CreateTeacherAsync(teacherCreateDto))
            .ReturnsAsync(createdTeacher);

        _sut = new TeachersController(_mockServiceManager.Object);

        // Act
        var result = await _sut.CreateTeacher(teacherCreateDto);
        var createdRouteResult = (CreatedAtRouteResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(createdRouteResult.RouteName, Is.EqualTo("TeacherById"));
            Assert.That(createdRouteResult.RouteValues["id"], Is.EqualTo(createdTeacher.Id));
        });
    }

    [Test]
    public async Task CreateTeacher_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        TeacherCreateDto? teacherCreateDto = null;

        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);
        _sut = new TeachersController(_mockServiceManager.Object);

        // Act
        var result = await _sut.CreateTeacher(teacherCreateDto);
        var badRequestResult = (BadRequestObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(TeacherCreateDto)} object is null"));
        });
    }

    [TestCase(1)]
    public async Task UpdateTeacher_ShouldReturnNoContentResult_OnSuccess(int teacherId)
    {
        // Arrange
        var teacherUpdateDto = GetTeacherUpdateDto();

        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);

        _sut = new TeachersController(_mockServiceManager.Object);

        // Act
        var result = await _sut.UpdateTeacher(teacherId, teacherUpdateDto);
        var noContentResult = (NoContentResult)result;

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task UpdateTeacher_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        TeacherUpdateDto? teacherUpdateDto = null;

        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);
        _sut = new TeachersController(_mockServiceManager.Object);

        // Act
        var result = await _sut.UpdateTeacher(1, teacherUpdateDto);
        var badRequestResult = (BadRequestObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(TeacherUpdateDto)} object is null"));
        });
    }

    [TestCase(1, 1)]
    public async Task ResignTeacherFromCourse_ShouldReturnNoContentResult_OnSuccess(int teacherId, int courseId)
    {
        // Arrange
        var teacherUpdateDto = GetTeacherUpdateDto();

        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);
        _mockTeacherService.Setup(s => s.ResignTeacherFromCourse(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<TeacherUpdateDto>(),
            true)).Verifiable();
            

        _sut = new TeachersController(_mockServiceManager.Object);

        // Act
        var result = await _sut.ResignTeacherFromCourse(teacherId, courseId, teacherUpdateDto, true);
        var noContentResult = (NoContentResult)result;

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task ResignTeacherFromCourse_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        TeacherUpdateDto? teacherUpdateDto = null;

        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);
        _sut = new TeachersController(_mockServiceManager.Object);

        // Act
        var result = await _sut.ResignTeacherFromCourse(1, 1, teacherUpdateDto, true);
        var badRequestResult = (BadRequestObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(TeacherUpdateDto)} object is null"));
        });
    }

    [Test]
    public async Task DeleteTeacher_ShouldReturnNoContentResult_IfTeacherIsFound()
    {
        // Arrange
        _mockServiceManager.Setup(s => s.TeacherService).Returns(_mockTeacherService.Object);
        _mockTeacherService.Setup(s => s.DeleteTeacherAsync(1, false))
            .Verifiable();

        _sut = new TeachersController(_mockServiceManager.Object);

        var result = await _sut.DeleteTeacher(1);
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    private static TeacherUpdateDto GetTeacherUpdateDto()
    {
        return new TeacherUpdateDto()
        {
            Name = "John Smith"
        };
    }

    private static TeacherCreateDto GetTeacherCreateDto()
    {
        return new TeacherCreateDto()
        {
            Name = "John Smith"
        };
    }

    private static List<TeacherShortResponseDto> GetTeacherShortResponseDtos(int countToReturn)
    {
        return countToReturn switch
        {
            0 => [],
            _ =>
                [
                    new (1, "John Smith"),
                    new (2, "John Baker")
                ],
        };
    }

    private static TeacherResponseDto GetTeacherResponseDto()
    {
        return new(1, "John Smith", null);
    }

    private static TeacherShortResponseDto GetTeacherShortResponseDto()
    {
        return new(1, "John Smith");
    }
}
