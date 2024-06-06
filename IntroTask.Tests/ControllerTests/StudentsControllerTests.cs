using IntroTask.Presentation.Controllers;
using IntroTaskWebApi.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Contracts;
using Shared.Dtos.CourseDtos;
using Shared.Dtos.StudentDtos;

namespace IntroTask.Tests.ControllerTests;

public class StudentsControllerTests
{
    private Mock<IServiceManager> _mockServiceManager;
    private Mock<IStudentService> _mockStudentService;
    private StudentsController _sut;

    [SetUp]
    public void Setup()
    {
        _mockServiceManager = new Mock<IServiceManager>();
        _mockStudentService = new Mock<IStudentService>();
    }

    [Test]
    public async Task GetStudents_ShouldReturnOkObjectResult()
    {
        // Arrange

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _mockStudentService.Setup(s => s.GetAllStudentsAsync(false))
            .ReturnsAsync(GetStudentShortResponseDtos(2));

        _sut = new StudentsController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetStudents();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [TestCase(2)]
    [TestCase(0)]
    public async Task GetStudents_ShouldReturnListOfStudents(int countToReturn)
    {
        // Arrange

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _mockStudentService.Setup(s => s.GetAllStudentsAsync(false))
            .ReturnsAsync(GetStudentShortResponseDtos(countToReturn));

        _sut = new StudentsController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetStudents();
        var okObjectResult = (OkObjectResult)result;

        // Assert
        Assert.That(okObjectResult.Value, Is.TypeOf<List<StudentShortResponseDto>>());
    }
    [TestCase(1)]
    public async Task GetStudent_ShouldReturnOkObjectResult_WhenStudentIsFound(int id)
    {
        // Arrange
        var expected = GetStudentResponseDto();

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _mockStudentService.Setup(s => s.GetStudentByIdAsync(id, false))
            .ReturnsAsync(GetStudentResponseDto());

        _sut = new StudentsController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetStudent(id);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }


    [TestCase(1)]
    public async Task GetStudent_ShouldReturnTeacher_WhenStudentIsFound(int id)
    {
        // Arrange
        var expected = GetStudentResponseDto();

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _mockStudentService.Setup(s => s.GetStudentByIdAsync(id, false))
            .ReturnsAsync(GetStudentResponseDto());

        _sut = new StudentsController(_mockServiceManager.Object);

        // Act
        var result = await _sut.GetStudent(id);
        var okObjectResult = (OkObjectResult)result;

        // Assert
        Assert.That(okObjectResult.Value, Is.EqualTo(expected));
    }

    [Test]
    public async Task CreateStudent_ShouldReturnCreatedAtRoute_OnSuccess()
    {
        // Arrange
        var createDto = GetStudentCreateDto();

        var createdResronseDto = GetStudentShortResponseDto();

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _mockStudentService.Setup(s => s.CreateStudentAsync(createDto))
            .ReturnsAsync(createdResronseDto);

        _sut = new StudentsController(_mockServiceManager.Object);

        // Act
        var result = await _sut.CreateStudent(createDto);
        var createdRouteResult = (CreatedAtRouteResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(createdRouteResult.RouteName, Is.EqualTo("StudentById"));
            Assert.That(createdRouteResult.RouteValues["id"], Is.EqualTo(createdResronseDto.Id));
        });
    }

    [Test]
    public async Task CreateStudent_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        StudentCreateDto? createDto = null;

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _sut = new StudentsController(_mockServiceManager.Object);

        // Act
        var result = await _sut.CreateStudent(createDto);
        var badRequestResult = (BadRequestObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(StudentCreateDto)} object is null"));
        });
    }

    [Test]
    public async Task CreateStudent_ShouldReturnUnprocessableEntityObjectResult_IfInvalidModel()
    {
        // Arrange
        var createDto = GetStudentCreateDto();

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _sut = new StudentsController(_mockServiceManager.Object);
        _sut.ModelState.AddModelError("Dto", "Invalid");

        // Act
        var result = await _sut.CreateStudent(createDto);
        var unprocessableEntityObjectResult = (UnprocessableEntityObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<UnprocessableEntityObjectResult>());
            Assert.That(unprocessableEntityObjectResult.Value, Is.TypeOf<SerializableError>());
        });
    }

    [TestCase(1)]
    public async Task UpdateStudent_ShouldReturnNoContentResult_OnSuccess(int id)
    {
        // Arrange
        var updateDto = GetStudentUpdateDto();

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _sut = new StudentsController(_mockServiceManager.Object);

        // Act
        var result = await _sut.UpdateStudent(id, updateDto);
        var noContentResult = (NoContentResult)result;

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task UpdateStudent_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        StudentUpdateDto? updateDto = null;

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _sut = new StudentsController(_mockServiceManager.Object);

        // Act
        var result = await _sut.UpdateStudent(1, updateDto);
        var badRequestResult = (BadRequestObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(StudentUpdateDto)} object is null"));
        });
    }

    [TestCase(1)]
    public async Task UpdateStudent_ShouldReturnUnprocessableEntityObjectResult_IfInvalidModel(int id)
    {
        // Arrange
        var updateDto = GetStudentUpdateDto();

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _sut = new StudentsController(_mockServiceManager.Object);
        _sut.ModelState.AddModelError("Dto", "Invalid");

        // Act
        var result = await _sut.UpdateStudent(id, updateDto);
        var unprocessableEntityObjectResult = (UnprocessableEntityObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<UnprocessableEntityObjectResult>());
            Assert.That(unprocessableEntityObjectResult.Value, Is.TypeOf<SerializableError>());
        });
    }

    [TestCase(1, 1)]
    public async Task EnrollStudentInCourse_ShouldReturnNoContentResult_OnSuccess(int studentId, int courseId)
    {
        // Arrange
        var updateDto = GetStudentUpdateDto();

        _mockServiceManager.Setup(s => s.StudentService).Returns(_mockStudentService.Object);

        _mockStudentService.Setup(s => s.EnrollStudentInCourseAsync(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<StudentUpdateDto>(),
            true)).Verifiable();


        _sut = new StudentsController(_mockServiceManager.Object);

        // Act
        var result = await _sut.EnrollStudentInCourse(studentId, courseId, updateDto);
        var noContentResult = (NoContentResult)result;

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task EnrollStudentInCourse_ShouldReturnBadRequestObjectResult_IfDtoIsNull()
    {
        // Arrange
        StudentUpdateDto? updateDto = null;

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _sut = new StudentsController(_mockServiceManager.Object);

        // Act
        var result = await _sut.EnrollStudentInCourse(1, 1, updateDto);
        var badRequestResult = (BadRequestObjectResult)result;

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badRequestResult.Value, Is.EqualTo($"{nameof(StudentUpdateDto)} object is null"));
        });
    }

    [TestCase(1, 1)]
    public async Task EnrollStudentInCourse_ShouldReturnUnprocessableEntityObjectResult_IfInvalidModel(int studentId, int courseId)
    {
        // Arrange
        var updateDto = GetStudentUpdateDto();

        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _sut = new StudentsController(_mockServiceManager.Object);

        _sut.ModelState.AddModelError("Dto", "Invalid");

        // Act
        var result = await _sut.EnrollStudentInCourse(studentId, courseId, updateDto);
        var unprocessableEntityObjectResult = (UnprocessableEntityObjectResult)result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<UnprocessableEntityObjectResult>());
            Assert.That(unprocessableEntityObjectResult.Value, Is.TypeOf<SerializableError>());
        });
    }

    [Test]
    public async Task DeleteStudent_ShouldReturnNoContentResult_IfStudentIsFound()
    {
        // Arrange
        _mockServiceManager.Setup(s => s.StudentService)
            .Returns(_mockStudentService.Object);

        _mockStudentService.Setup(s => s.DeleteStudentAsync(1, false))
            .Verifiable();

        _sut = new StudentsController(_mockServiceManager.Object);

        var result = await _sut.DeleteStudent(1);
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    private static StudentUpdateDto GetStudentUpdateDto()
    {
        return new StudentUpdateDto()
        {
            FirstName = "John",
            LastName = "Smith"
        };
    }

    private static StudentCreateDto GetStudentCreateDto()
    {
        return new StudentCreateDto()
        {
            FirstName = "John",
            LastName = "Smith"
        };
    }

    private static List<StudentShortResponseDto> GetStudentShortResponseDtos(
        int countToReturn)
    {
        return countToReturn switch
        {
            0 => [],
            _ =>
                [
                    new (1, "John", "Smith"),
                    new (2, "John", "Baker")
                ],
        };
    }

    private static StudentResponseDto GetStudentResponseDto()
    {
        return new(1, "John", "Smith", null);
    }

    private static StudentShortResponseDto GetStudentShortResponseDto()
    {
        return new(1, "John", "Smith");
    }
}
