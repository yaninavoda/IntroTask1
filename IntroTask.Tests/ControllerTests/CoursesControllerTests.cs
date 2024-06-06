using IntroTaskWebApi.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Contracts;
using Shared.Dtos.CourseDtos;

namespace IntroTask.Tests.ControllerTests;

public class CoursesControllerTests
{
    private Mock<IServiceManager> _mockServiceManager;
    private Mock<ICourseService> _mockCourseService;
    private CoursesController _sut;

    [SetUp]
    public void Setup()
    {
        _mockServiceManager = new Mock<IServiceManager>();
        _mockCourseService = new Mock<ICourseService>();
    }

    [Test]
    public async Task GetCourses_ShouldReturnOkObjectResult()
    {
        // Arrange

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _mockCourseService.Setup(s => s.GetAllCoursesAsync(false))
            .ReturnsAsync(GetCourseShortResponseDtos(2));

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetCourses();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [TestCase(2)]
    [TestCase(0)]
    public async Task GetCourses_ShouldReturnListOfCourses(int countToReturn)
    {
        // Arrange

        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);

        _mockCourseService.Setup(s => s.GetAllCoursesAsync(false))
            .ReturnsAsync(GetCourseShortResponseDtos(countToReturn));

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetCourses();
        var okObjectResult = (OkObjectResult)result;

        // Assert
        Assert.That(okObjectResult.Value, Is.TypeOf<List<CourseShortResponseDto>>());
    }
    [TestCase(1)]
    public async Task GetCourse_ShouldReturnOkObjectResult_WhenCourseIsFound(int id)
    {
        // Arrange
        var expected = GetCourseResponseDto();

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _mockCourseService.Setup(s => s.GetCourseByIdAsync(id, false))
            .ReturnsAsync(GetCourseResponseDto());

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetCourse(id);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }


    [TestCase(1)]
    public async Task GetCourse_ShouldReturnCourse_WhenCourseIsFound(int id)
    {
        // Arrange
        var expected = GetCourseResponseDto();

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _mockCourseService.Setup(s => s.GetCourseByIdAsync(id, false))
            .ReturnsAsync(GetCourseResponseDto());

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetCourse(id);
        var okObjectResult = (OkObjectResult)result;

        // Assert
        Assert.That(okObjectResult.Value, Is.EqualTo(expected));
    }

    [Test]
    public async Task CreateCourse_ShouldReturnCreatedAtRoute_OnSuccess()
    {
        // Arrange
        var createDto = GetCourseCreateDto();

        var createdResronseDto = GetCourseShortResponseDto();

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _mockCourseService.Setup(s => s.CreateCourseAsync(createDto))
            .ReturnsAsync(createdResronseDto);

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.CreateCourse(createDto);
        var createdRouteResult = (CreatedAtRouteResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(createdRouteResult.RouteName, Is.EqualTo("CourseById"));
            Assert.That(createdRouteResult.RouteValues["id"], Is.EqualTo(createdResronseDto.Id));
        });
    }

    [Test]
    public async Task CreateCourse_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        CourseCreateDto? createDto = null;

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.CreateCourse(createDto);
        var badRequestResult = (BadRequestObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(CourseCreateDto)} object is null"));
        });
    }

    [Test]
    public async Task CreateCourse_ShouldReturnUnprocessableEntityObjectResult_IfInvalidModel()
    {
        // Arrange
        var createDto = GetCourseCreateDto();

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _sut = new CoursesController(_mockServiceManager.Object);
        _sut.ModelState.AddModelError("Dto", "Invalid");

        // Act
        var result = await _sut.CreateCourse(createDto);
        var unprocessableEntityObjectResult = (UnprocessableEntityObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<UnprocessableEntityObjectResult>());
            Assert.That(unprocessableEntityObjectResult.Value, Is.TypeOf<SerializableError>());
        });
    }

    [TestCase(1)]
    public async Task UpdateCourse_ShouldReturnNoContentResult_OnSuccess(int id)
    {
        // Arrange
        var updateDto = GetCourseUpdateDto();

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.UpdateCourse(id, updateDto);
        var noContentResult = (NoContentResult)result;

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [TestCase(1)]
    public async Task UpdateCourse_ShouldReturnUnprocessableEntityObjectResult_IfInvalidModel(int id)
    {
        // Arrange
        var updateDto = GetCourseUpdateDto();

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _sut = new CoursesController(_mockServiceManager.Object);
        _sut.ModelState.AddModelError("Dto", "Invalid");

        // Act
        var result = await _sut.UpdateCourse(id, updateDto);
        var unprocessableEntityObjectResult = (UnprocessableEntityObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<UnprocessableEntityObjectResult>());
            Assert.That(unprocessableEntityObjectResult.Value, Is.TypeOf<SerializableError>());
        });
    }

    [Test]
    public async Task UpdateCourse_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        CourseUpdateDto? updateDto = null;

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.UpdateCourse(1, updateDto);
        var badRequestResult = (BadRequestObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(CourseUpdateDto)} object is null"));
        });
    }

    [TestCase(1, 1)]
    public async Task ExcludeStudentFromCourse_ShouldReturnNoContentResult_OnSuccess(int courseId, int studentId)
    {
        // Arrange
        var updateDto = GetCourseUpdateDto();

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _mockCourseService.Setup(s => s.ExcludeStudentFromCourse(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CourseUpdateDto>(),
            true)).Verifiable();

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.ExcludeStudentFromCourse(courseId, studentId, updateDto);
        var noContentResult = (NoContentResult)result;

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task ExcludeStudentFromCourse_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        CourseUpdateDto? updateDto = null;

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.ExcludeStudentFromCourse(1, 1, updateDto);
        var badRequestResult = (BadRequestObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(CourseUpdateDto)} object is null"));
        });
    }

    [TestCase(1, 1)]
    public async Task ExcludeStudentFromCourse_ShouldReturnUnprocessableEntityObjectResult_IfInvalidModel(int courseId, int studentId)
    {
        // Arrange
        var updateDto = GetCourseUpdateDto();

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _sut = new CoursesController(_mockServiceManager.Object);
        _sut.ModelState.AddModelError("Dto", "Invalid");

        // Act
        var result = await _sut.ExcludeStudentFromCourse(courseId, studentId, updateDto);
        var unprocessableEntityObjectResult = (UnprocessableEntityObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<UnprocessableEntityObjectResult>());
            Assert.That(unprocessableEntityObjectResult.Value, Is.TypeOf<SerializableError>());
        });
    }

    [TestCase(1, 1)]
    public async Task AppointTeacherForCourse_ShouldReturnNoContentResult_OnSuccess(int courseId, int teacherId)
    {
        // Arrange
        var updateDto = GetCourseUpdateDto();

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _mockCourseService.Setup(s => s.AppointTeacherForCourse(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CourseUpdateDto>(),
            true)).Verifiable();

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.ExcludeStudentFromCourse(courseId, teacherId, updateDto);
        var noContentResult = (NoContentResult)result;

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task AppointTeacherForCourse_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        CourseUpdateDto? updateDto = null;

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.AppointTeacherForCourse(1, 1, updateDto);
        var badRequestResult = (BadRequestObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(CourseUpdateDto)} object is null"));
        });
    }

    [TestCase(1, 1)]
    public async Task AppointTeacherForCourse_ShouldReturnUnprocessableEntityObjectResult_IfInvalidModel(int courseId, int teacherId)
    {
        // Arrange
        var updateDto = GetCourseUpdateDto();

        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _sut = new CoursesController(_mockServiceManager.Object);
        _sut.ModelState.AddModelError("Dto", "Invalid");

        // Act
        var result = await _sut.ExcludeStudentFromCourse(courseId, teacherId, updateDto);
        var unprocessableEntityObjectResult = (UnprocessableEntityObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<UnprocessableEntityObjectResult>());
            Assert.That(unprocessableEntityObjectResult.Value, Is.TypeOf<SerializableError>());
        });
    }

    [Test]
    public async Task DeleteCourse_ShouldReturnNoContentResult_IfCourseIsFound()
    {
        // Arrange
        _mockServiceManager.Setup(s => s.CourseService)
            .Returns(_mockCourseService.Object);

        _mockCourseService.Setup(s => s.DeleteCourseAsync(1, false))
            .Verifiable();

        _sut = new CoursesController(_mockServiceManager.Object);

        var result = await _sut.DeleteCourse(1);
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    private static CourseUpdateDto GetCourseUpdateDto()
    {
        return new CourseUpdateDto()
        {
            Title = "Course"
        };
    }

    private static CourseCreateDto GetCourseCreateDto()
    {
        return new CourseCreateDto()
        {
            Title = "Course"
        };
    }

    private static List<CourseShortResponseDto> GetCourseShortResponseDtos(int countToReturn)
    {
        return countToReturn switch
        {
            0 => [],
            _ =>
                [
                    new (1, "Course 1"),
                    new (2, "Course 2")
                ],
        };
    }

    private static CourseResponseDto GetCourseResponseDto()
    {
        return new(1, "Course", null, null);
    }

    private static CourseShortResponseDto GetCourseShortResponseDto()
    {
        return new(1, "Course");
    }
}
