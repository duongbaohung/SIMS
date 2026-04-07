using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS.DatabaseContext;
using SIMS.DatabaseContext.Entities;
using SIMS.Models;
using SIMS.Services.Interfaces;
using System.Security.Claims;

namespace SIMS.Controllers
{
    [Authorize]
    public class GradeController : Controller
    {
        private readonly SimsDbContext _context;
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;

        public GradeController(SimsDbContext context, IUserService userService, ICourseService courseService)
        {
            _context = context;
            _userService = userService;
            _courseService = courseService;
        }

        // GET: /Grade/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var username = User.Identity?.Name;

            IQueryable<Grade> query = _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Course);

            // Students only see their own grades
            if (role == "Student")
            {
                query = query.Where(g => g.Student != null && g.Student.Username == username);
            }

            var grades = await query.ToListAsync();
            return View(grades);
        }

        // GET: /Grade/Create
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Create()
        {
            var students = await _userService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();

            var viewModel = new GradeViewModel
            {
                Students = students.Select(u => new StudentItem { Id = u.Id, FullName = u.Username }).ToList(),
                Courses = courses.Select(c => new CourseItem { Id = c.Id, Title = c.CourseName }).ToList()
            };

            return View(viewModel);
        }

        // POST: /Grade/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Create(GradeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var grade = new Grade
                {
                    StudentId = model.StudentId,
                    CourseId = model.CourseId,
                    Score = model.Score,
                    Remarks = model.Remarks,
                    CreatedAt = DateTime.Now
                };

                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Grade submitted successfully!";
                return RedirectToAction("Index");
            }

            // Re-populate lists if validation fails
            var students = await _userService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();
            model.Students = students.Select(u => new StudentItem { Id = u.Id, FullName = u.Username }).ToList();
            model.Courses = courses.Select(c => new CourseItem { Id = c.Id, Title = c.CourseName }).ToList();

            return View(model);
        }
    }
}