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

        // GET: /Course/Assign
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Assign()
        {
            // Fetch the required data from our services
            var students = await _userService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();

            // Populate the ViewModel for the View
            var viewModel = new EnrollmentViewModel
            {
                // Updated to use Username instead of FullName
                Students = students.Select(u => new StudentItem
                {
                    Id = u.Id,
                    FullName = u.Username
                }).ToList(),
                // Updated to use CourseName instead of Title
                Courses = courses.Select(c => new CourseItem
                {
                    Id = c.Id,
                    Title = c.CourseName
                }).ToList()
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
                // Execute the enrollment logic through the service
                var success = await _courseService.EnrollStudentAsync(model.StudentId, model.CourseId);

                if (success)
                {
                    TempData["SuccessMessage"] = "Student assigned to course successfully!";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Unable to assign student. Please check if they are already enrolled.");
            }

            // Re-populate lists if validation fails or logic fails
            var students = await _userService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();

            // Consistent mapping for the re-population
            model.Students = students.Select(u => new StudentItem { Id = u.Id, FullName = u.Username }).ToList();
            model.Courses = courses.Select(c => new CourseItem { Id = c.Id, Title = c.CourseName }).ToList();

            return View(model);
        }
    }
}