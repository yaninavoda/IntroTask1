using AutoMapper;
using Contracts;
using Entities.Exceptions;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Service;
using Shared.Dtos.CourseDtos;
using System.Linq.Expressions;

namespace IntroTask.Tests.ServiceTests.CourseServiceTests
{
    public class ExcludeStudentFromCourseTests
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

        [TestCase(1, 1, true)]
        [TestCase(1, 1, false)]
        public async Task ExcludeStudentFromCourse_ShouldThrowException_IfStudentIsNotFound(int id, int studentId, bool trackChanges)
        {
            // Arrange
            SetupMocksStudentNotFound(id);

            _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<StudentNotFoundException>(async () => await _sut.ExcludeStudentFromCourse(id, studentId, GetCourseUpdateDto(), trackChanges));
        }

        [TestCase(1, 1, true)]
        [TestCase(1, 1, false)]
        public async Task ExcludeStudentFromCourse_ShouldThrowException_IfCourseIsNotFound(
            int id, int studentId, bool trackChanges)
        {
            // Arrange
            SetupMocksCourseNotFound(id);

            _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<CourseNotFoundException>(async () => await _sut.ExcludeStudentFromCourse(id, studentId, GetCourseUpdateDto(), trackChanges));
        }

        private static CourseUpdateDto GetCourseUpdateDto()
        {
            return new CourseUpdateDto()
            {
                Title = "Course 1"
            };
        }

        private static Student GetStudent()
        {
            return new Student
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
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

        private void SetupMocksStudentNotFound(int id)
        {
            _repositoryMock.Setup(repo => repo.Student.GetSingleOrDefaultAsync(
                AnyEntityPredicate<Student>(),
                AnyEntityInclude<Student>(),
                It.IsAny<bool>()))
                    .ThrowsAsync(new StudentNotFoundException(id));
        }

        private void SetupMocksCourseNotFound(int id)
        {
            _repositoryMock.Setup(repo => repo.Student.GetSingleOrDefaultAsync(
                AnyEntityPredicate<Student>(),
                AnyEntityInclude<Student>(),
                It.IsAny<bool>()))
                    .ReturnsAsync(GetStudent());

            _repositoryMock.Setup(repo => repo.Course.GetSingleOrDefaultAsync(
                AnyEntityPredicate<Course>(),
                AnyEntityInclude<Course>(),
                It.IsAny<bool>()))
                    .ThrowsAsync(new CourseNotFoundException(id));
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
}
