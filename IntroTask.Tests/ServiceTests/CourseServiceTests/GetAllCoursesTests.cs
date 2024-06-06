using AutoMapper;
using Contracts;
using IntroTask.Entities;
using Moq;
using Service;
using Shared.Dtos.CourseDtos;
using Shared.Dtos.TeacherDtos;

namespace IntroTask.Tests.ServiceTests.CourseServiceTests
{
    public class GetAllCoursesTests
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

        [TestCase(true)]
        [TestCase(false)]
        public async Task GetAllCoursesAsync_ShouldReturnCourseShortResponseDtos_IfCoursesExist(bool trackChanges)
        {
            // Arrange
            SetupRepositoryMockReturnsDataCollection();
            SetupMapperMockReturnsDataCollection();

            _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

            // Act
            var courseDtos = await _sut.GetAllCoursesAsync(trackChanges);

            // Assert
            Assert.That(courseDtos, Is.InstanceOf<IEnumerable<CourseShortResponseDto>>());
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task GetAllCoursesAsync_ShouldReturnCorrectAmmountOfDtos_IfCoursesExist(bool trackChanges)
        {
            // Arrange
            SetupRepositoryMockReturnsDataCollection();
            SetupMapperMockReturnsDataCollection();

            _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

            // Act
            var courseDtos = await _sut.GetAllCoursesAsync(trackChanges);

            // Assert
            Assert.That(courseDtos.Count, Is.EqualTo(GetCourses().Count()));
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task GetAllCoursesAsync_ShouldBeCalledOnce_IfCourseExist(bool trackChanges)
        {
            // Arrange
            SetupRepositoryMockReturnsDataCollection();
            SetupMapperMockReturnsDataCollection();

            _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

            // Act
            var courseDtos = await _sut.GetAllCoursesAsync(trackChanges);

            // Assert
            _repositoryMock.Verify(repo => repo.Course.GetAllCoursesAsync(trackChanges), Times.Once);
        }


        [Test]
        public async Task GetAllCoursesAsync_ShouldReturnEmptyList_IfNoCoursesFound()
        {
            // Arrange
            SetupRepositoryMockReturnsEmptyCollection();

            _sut = new CourseService(_repositoryMock.Object, _mapperMock.Object);

            // Act
            var courseDtos = await _sut.GetAllCoursesAsync(trackChanges: false);

            // Assert
            Assert.That(courseDtos, Is.Empty);
        }

        private static List<CourseShortResponseDto> GetCourseShortResponseDtos()
        {
            var dtos = new List<CourseShortResponseDto>
        {
            new (Id: 1, Title: "Course 1"),
            new (Id: 2, Title: "Course 2"),
        };

            return dtos;
        }

        private static List<Course> GetCourses()
        {
            var teachers = new List<Course>
        {
            new ()
            {
                Id = 1,
                Title = "Course 1"
            },
            new ()
            {
                Id = 2,
                Title = "Course 2"
            },
        };

            return teachers;
        }

        private void SetupMapperMockReturnsDataCollection()
        {
            _mapperMock.Setup(m => m.Map<List<CourseShortResponseDto>>(It.IsAny<IEnumerable<Course>>()))
                            .Returns(GetCourseShortResponseDtos());
        }

        private void SetupRepositoryMockReturnsDataCollection()
        {
            _repositoryMock.Setup(repo => repo.Course.GetAllCoursesAsync(It.IsAny<bool>()))
                .ReturnsAsync(GetCourses());
        }

        private void SetupRepositoryMockReturnsEmptyCollection()
        {
            _repositoryMock.Setup(repo => repo.Course.GetAllCoursesAsync(It.IsAny<bool>()))
                    .ReturnsAsync(new List<Course>());

            _mapperMock.Setup(m => m.Map<List<CourseShortResponseDto>>(It.IsAny<IEnumerable<Course>>()))
                .Returns(new List<CourseShortResponseDto>());
        }
    }
}
