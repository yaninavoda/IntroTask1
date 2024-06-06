using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.StudentDtos;

namespace IntroTask.Tests.ServiceTests.StudentServiceTests;

public class EnrollStudentInCourseTests
{
    private Mock<IRepositoryManager> _repositoryMock;
    private Mock<IMapper> _mapperMock;
    private StudentService? _sut;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepositoryManager>();
        _mapperMock = new Mock<IMapper>();
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task EnrollStudentInCourse_ShouldThrowException_IfStudentIsNotFound(int studentId, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksStudentNotFound(studentId);

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<StudentNotFoundException>(async () => await _sut.EnrollStudentInCourseAsync(studentId, courseId, GetStudentUpdateDto(), trackChanges));
    }

    [TestCase(1, 1, true)]
    [TestCase(1, 1, false)]
    public async Task EnrollStudentInCourse_ShouldThrowException_IfCourseIsNotFound(
        int studentId, int courseId, bool trackChanges)
    {
        // Arrange
        SetupMocksCourseNotFound(courseId);

        _sut = new StudentService(_repositoryMock.Object, _mapperMock.Object);

        // Act & Assert
        Assert.ThrowsAsync<CourseNotFoundException>(async () => await _sut.EnrollStudentInCourseAsync(studentId, courseId, GetStudentUpdateDto(), trackChanges));
    }

    private void SetupMocksStudentNotFound(int id)
    {
        _repositoryMock.Setup(repo => repo.Student.GetStudentByIdAsync(
            It.IsAny<int>(),
            It.IsAny<bool>()))
                .ThrowsAsync(new StudentNotFoundException(id));
    }

    private void SetupMocksCourseNotFound(int id)
    {
        _repositoryMock.Setup(repo => repo.Student.GetStudentByIdAsync(
            It.IsAny<int>(),
            It.IsAny<bool>()))
                .ReturnsAsync(GetStudentWithCourse());

        _repositoryMock.Setup(repo => repo.Course.GetCourseByIdAsync(
            It.IsAny<int>(),
            It.IsAny<bool>()))
                .ThrowsAsync(new CourseNotFoundException(id));
    }

    private static Student GetStudentWithCourse()
    {
        return new Student
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Courses = new List<Course>
            {
                new ()
                {   Id = 1,
                    Title = "Course 1",
                    TeacherId = 1,
                    Students = new List<Student>
                    {
                        new () { Id = 1, FirstName = "Jane", LastName = "Doe"}
                    }
                }
            }
        };
    }

    private static StudentUpdateDto GetStudentUpdateDto()
    {
        return new StudentUpdateDto()
        {
            FirstName = "Jane",
            LastName = "Doe"
        };
    }
}
