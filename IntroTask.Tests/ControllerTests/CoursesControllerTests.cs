using IntroTask.Presentation.Controllers;
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
    public async Task GetStudents_ShouldReturnOkObjectResult()
    {
        // Arrange

        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);
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

        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);
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

        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);
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

        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);
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

        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);
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

    [TestCase(1)]
    public async Task UpdateCourse_ShouldReturnNoContentResult_OnSuccess(int id)
    {
        // Arrange
        var updateDto = GetCourseUpdateDto();

        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);

        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        var result = await _sut.UpdateCourse(id, updateDto);
        var noContentResult = (NoContentResult)result;

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task UpdateCourse_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        CourseUpdateDto? updateDto = null;

        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);
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

    // TODO: write ExcludeStudentFromCourse tests
    // TODO: write AppointTeacherForCourse tests
    [TestCase(1, 1)]
    public async Task ResignTeacherFromCourse_ShouldReturnNoContentResult_OnSuccess(int studentId, int courseId)
    {
        // Arrange
        var updateDto = GetCourseUpdateDto();

        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);
        //_mockStudentService.Setup(s => s.ResignTeacherFromCourse(
        //    It.IsAny<int>(),
        //    It.IsAny<int>(),
        //    It.IsAny<TeacherUpdateDto>(),
        //    true)).Verifiable();


        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        //var result = await _sut.ResignTeacherFromCourse(teacherId, courseId, teacherUpdateDto, true);
        //var noContentResult = (NoContentResult)result;

        // Assert
        //Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task ResignTeacherFromCourse_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        //StudentUpdateDto? updateDto = null;

        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);
        _sut = new CoursesController(_mockServiceManager.Object);

        // Act
        //var result = await _sut.ResignTeacherFromCourse(1, 1, updateDto, true);
        //var badRequestResult = (BadRequestObjectResult)result;

        //// Assert
        //Assert.Multiple(() =>
        //{
        //    Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        //    Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(StudentUpdateDto)} object is null"));
        //});
    }

    [Test]
    public async Task DeleteCourse_ShouldReturnNoContentResult_IfCourseIsFound()
    {
        // Arrange
        _mockServiceManager.Setup(s => s.CourseService).Returns(_mockCourseService.Object);
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
