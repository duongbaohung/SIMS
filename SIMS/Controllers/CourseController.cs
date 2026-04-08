using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.DatabaseContext.Entities;
using SIMS.Models;
using SIMS.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace SIMS.Controllers
{
    [Authorize]
    [Route("Course")] // Standardizes all routes to start with /Course
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;

        public CourseController(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        // GET: /Course/Index or /Course
        [HttpGet]
        [HttpGet("Index")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        // GET: /Course/Details/CS101
        [HttpGet("Details/{courseCode}")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public async Task<IActionResult> Details(string courseCode)
        {
            if (string.IsNullOrEmpty(courseCode)) return NotFound();

            var course = await _courseService.GetCourseByCodeAsync(courseCode);
            if (course == null) return NotFound();

            return View(course);
        }

        // GET: /Course/Add
        [HttpGet("Add")]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Course/Add
        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Course model)
        {
            ModelState.Remove("Description");

            if (ModelState.IsValid)
            {
                var existingCourse = await _courseService.GetCourseByCodeAsync(model.CourseCode);
                if (existingCourse != null)
                {
                    ModelState.AddModelError("CourseCode", "A course with this code already exists.");
                    return View(model);
                }

                await _courseService.AddCourseAsync(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Course/Edit/CS101
        // FIXED: This attribute now correctly maps 'asp-route-courseCode' to the URL path
        [HttpGet("Edit/{courseCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string courseCode)
        {
            if (string.IsNullOrEmpty(courseCode)) return NotFound();

            var course = await _courseService.GetCourseByCodeAsync(courseCode);
            if (course == null) return NotFound();

            return View(course);
        }

        // POST: /Course/Edit/CS101
        [HttpPost("Edit/{courseCode}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string courseCode, Course model)
        {
            if (courseCode != model.CourseCode) return BadRequest();

            ModelState.Remove("Description");

            if (ModelState.IsValid)
            {
                var success = await _courseService.UpdateCourseAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Course updated successfully.";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Unable to save changes. Please try again.");
            }

            return View(model);
        }

        // GET: /Course/Delete/CS101
        [HttpGet("Delete/{courseCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string courseCode)
        {
            if (string.IsNullOrEmpty(courseCode)) return NotFound();

            var course = await _courseService.GetCourseByCodeAsync(courseCode);
            if (course == null) return NotFound();

            return View(course);
        }

        // POST: /Course/Delete/CS101
        [HttpPost("Delete/{courseCode}")]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string courseCode)
        {
            var course = await _courseService.GetCourseByCodeAsync(courseCode);
            if (course != null)
            {
                await _courseService.DeleteCourseAsync(course.Id);
            }
            return RedirectToAction("Index");
        }

        // GET: /Course/Assign
        [HttpGet("Assign")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Assign()
        {
            var students = await _userService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();
            var recentEnrollments = await _courseService.GetRecentEnrollmentsAsync(10);

            var viewModel = new EnrollmentViewModel
            {
                Students = students.Select(u => new StudentItem { Id = u.Id, FullName = u.Username }).ToList(),
                Courses = courses.Select(c => new CourseItem { Id = c.Id, Title = c.CourseName }).ToList(),
                Enrollments = recentEnrollments.Select(e => new EnrollmentDto
                {
                    StudentName = students.FirstOrDefault(s => s.Id == e.StudentId)?.Username ?? "Unknown",
                    CourseName = courses.FirstOrDefault(c => c.Id == e.CourseId)?.CourseName ?? "Unknown"
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: /Course/Assign
        [HttpPost("Assign")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Assign(EnrollmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _courseService.EnrollStudentAsync(model.StudentId, model.CourseId);
                if (success)
                {
                    TempData["SuccessMessage"] = "Student assigned successfully!";
                    return RedirectToAction("Assign");
                }
                ModelState.AddModelError("", "Enrollment failed. Ensure the course is open and has capacity.");
            }

            var students = await _userService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();
            var recentEnrollments = await _courseService.GetRecentEnrollmentsAsync(10);

            model.Students = students.Select(u => new StudentItem { Id = u.Id, FullName = u.Username }).ToList();
            model.Courses = courses.Select(c => new CourseItem { Id = c.Id, Title = c.CourseName }).ToList();
            model.Enrollments = recentEnrollments.Select(e => new EnrollmentDto
            {
                StudentName = students.FirstOrDefault(s => s.Id == e.StudentId)?.Username ?? "Unknown",
                CourseName = courses.FirstOrDefault(c => c.Id == e.CourseId)?.CourseName ?? "Unknown"
            }).ToList();

            return View(model);
        }
    }
}