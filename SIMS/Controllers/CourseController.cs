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
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;

        public CourseController(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        // GET: /Course/Index
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        // GET: /Course/Details/CS101
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public async Task<IActionResult> Details(string courseCode)
        {
            if (string.IsNullOrEmpty(courseCode)) return NotFound();

            var course = await _courseService.GetCourseByCodeAsync(courseCode);
            if (course == null) return NotFound();

            return View(course);
        }

        // GET: /Course/Add
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Course/Add
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Course model)
        {
            ModelState.Remove("Description");

            if (ModelState.IsValid)
            {
                await _courseService.AddCourseAsync(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Course/Edit/CS101
        // Renamed 'id' to 'courseCode' to prevent conflicts with the numeric Model.Id
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string courseCode)
        {
            if (string.IsNullOrEmpty(courseCode)) return NotFound();

            var course = await _courseService.GetCourseByCodeAsync(courseCode);
            if (course == null) return NotFound();

            return View(course);
        }

        // POST: /Course/Edit/CS101
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string courseCode, Course model)
        {
            // Security check: ensure URL code matches model code
            if (courseCode != model.CourseCode) return BadRequest();

            ModelState.Remove("Description");

            if (ModelState.IsValid)
            {
                // The Repository now handles this safely using SetValues
                var success = await _courseService.UpdateCourseAsync(model);

                if (success)
                {
                    TempData["SuccessMessage"] = "Course updated successfully.";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Unable to save changes. The record might have been deleted or modified by another user.");
            }

            return View(model);
        }

        // GET: /Course/Delete/CS101
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string courseCode)
        {
            if (string.IsNullOrEmpty(courseCode)) return NotFound();

            var course = await _courseService.GetCourseByCodeAsync(courseCode);
            if (course == null) return NotFound();

            return View(course);
        }

        // POST: /Course/Delete/CS101
        [HttpPost, ActionName("Delete")]
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
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Assign()
        {
            var students = await _userService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();

            var viewModel = new EnrollmentViewModel
            {
                Students = students.Select(u => new StudentItem { Id = u.Id, FullName = u.Username }).ToList(),
                Courses = courses.Select(c => new CourseItem { Id = c.Id, Title = c.CourseName }).ToList()
            };

            return View(viewModel);
        }

        // POST: /Course/Assign
        [HttpPost]
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
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Enrollment failed. Check capacity or existing enrollments.");
            }

            var students = await _userService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();
            model.Students = students.Select(u => new StudentItem { Id = u.Id, FullName = u.Username }).ToList();
            model.Courses = courses.Select(c => new CourseItem { Id = c.Id, Title = c.CourseName }).ToList();

            return View(model);
        }
    }
}