using Moq;
using SIMS.DatabaseContext.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace SIMS.Tests
{
    public class ServiceTests
    {
        private readonly Mock<ICourseRepository> _courseRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly CourseService _courseService;
        private readonly UserService _userService;

        public ServiceTests()
        {
            // We mock the repositories so we don't need a real database for testing
            _courseRepoMock = new Mock<ICourseRepository>();
            _userRepoMock = new Mock<IUserRepository>();

            // Inject the mocks into the actual services
            _courseService = new CourseService(_courseRepoMock.Object);
            _userService = new UserService(_userRepoMock.Object);
        }

        #region Course Service Tests

        [Fact]
        public async Task AddCourseAsync_ShouldSetDefaultStatusAndEnrollment()
        {
            // Arrange
            var newCourse = new Course { CourseCode = "CS101", CourseName = "Intro", Capacity = 30 };
            _courseRepoMock.Setup(repo => repo.AddCourseAsync(It.IsAny<Course>())).ReturnsAsync(true);

            // Act
            var result = await _courseService.AddCourseAsync(newCourse);

            // Assert
            Assert.True(result);
            Assert.Equal(1, newCourse.Status); // Logic check: Must be Open (1)
            Assert.Equal(0, newCourse.Enrolled); // Logic check: Must start at 0
            _courseRepoMock.Verify(repo => repo.AddCourseAsync(newCourse), Times.Once);
        }

        [Fact]
        public async Task DeleteCourseAsync_ShouldFail_WhenIdIsZeroOrLess()
        {
            // Act
            var result = await _courseService.DeleteCourseAsync(0);

            // Assert
            Assert.False(result);
            _courseRepoMock.Verify(repo => repo.DeleteCourseAsync(It.IsAny<int>()), Times.Never);
        }

        #endregion

        #region Student Service Tests

        [Fact]
        public async Task AddStudentAsync_ShouldApplyBusinessRules()
        {
            // Arrange
            var student = new User { Username = "teststudent", Email = "test@test.com" };
            _userRepoMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>())).ReturnsAsync(true);

            // Act
            var result = await _userService.AddStudentAsync(student);

            // Assert
            Assert.True(result);
            Assert.Equal("Student", student.Role); // Verify role assignment
            Assert.Equal(1, student.Status); // Verify status assignment
            Assert.NotNull(student.HashPassword); // Verify default password
        }

        [Fact]
        public async Task DeleteStudentAsync_ShouldReturnFalse_ForInvalidId()
        {
            // Act
            var result = await _userService.DeleteStudentAsync(-1);

            // Assert
            Assert.False(result);
            _userRepoMock.Verify(repo => repo.DeleteUserAsync(It.IsAny<int>()), Times.Never);
        }

        #endregion
    }
}
