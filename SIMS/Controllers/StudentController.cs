using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.DatabaseContext.Entities;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IUserService _userService;

        // Inject the user service via constructor
        public StudentController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        // Usually, only Admins and Teachers should see the full student roster
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Index()
        {
            // Fetch students from the database and pass them to the View
            var students = await _userService.GetAllStudentsAsync();
            return View(students);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Only admins can add students
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(User model)
        {
            // IMPORTANT FIX: 
            // Remove validation for fields that are not in the HTML form.
            // Our UserService will set these automatically, so we don't care if they are empty right now.
            ModelState.Remove("HashPassword");
            ModelState.Remove("Role");

            // If the HTML form is valid, save the student and redirect to the list
            if (ModelState.IsValid)
            {
                await _userService.AddStudentAsync(model);
                return RedirectToAction("Index");
            }

            // If there are errors (like missing email), reload the page with the errors
            return View(model);
        }
    }
}