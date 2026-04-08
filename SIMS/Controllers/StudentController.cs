using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.DatabaseContext.Entities;
using SIMS.Services.Interfaces;
using System.Threading.Tasks;

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

        // GET: /Student/Index
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Index()
        {
            // Fetch users with the "Student" role and pass them to the View
            var students = await _userService.GetAllStudentsAsync();
            return View(students);
        }

        // GET: /Student/Add
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Student/Add
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(User model)
        {
            // Remove validation for fields handled internally to prevent silent failures
            ModelState.Remove("HashPassword");
            ModelState.Remove("Role");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("Status");
            ModelState.Remove("Id");
            ModelState.Remove("Enrollments");

            if (ModelState.IsValid)
            {
                // 1. Proactive check for Duplicate Username
                var existingUsername = await _userService.GetUserByUsernameAsync(model.Username);
                if (existingUsername != null)
                {
                    ModelState.AddModelError("Username", "This Username is already taken.");
                    return View(model);
                }

                // 2. Proactive check for Duplicate Email
                var existingEmail = await _userService.GetUserByEmailAsync(model.Email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("Email", "This Email address is already registered.");
                    return View(model);
                }

                await _userService.AddStudentAsync(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Student/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return NotFound();

            var student = await _userService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: /Student/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User model)
        {
            if (id != model.Id) return BadRequest();

            // Ignore fields that shouldn't be validated during a standard profile update
            ModelState.Remove("HashPassword");
            ModelState.Remove("Role");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("Status");
            ModelState.Remove("Enrollments");

            if (ModelState.IsValid)
            {
                // Check for duplicate username (excluding the current user's ID)
                var existingUsername = await _userService.GetUserByUsernameAsync(model.Username);
                if (existingUsername != null && existingUsername.Id != model.Id)
                {
                    ModelState.AddModelError("Username", "Username is already in use by another account.");
                    return View(model);
                }

                // Check for duplicate email (excluding the current user's ID)
                var existingEmail = await _userService.GetUserByEmailAsync(model.Email);
                if (existingEmail != null && existingEmail.Id != model.Id)
                {
                    ModelState.AddModelError("Email", "Email is already in use by another account.");
                    return View(model);
                }

                var success = await _userService.UpdateStudentAsync(model);
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Unable to save changes. Please try again.");
            }

            return View(model);
        }

        // GET: /Student/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return NotFound();

            var student = await _userService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _userService.DeleteStudentAsync(id);
            return RedirectToAction("Index");
        }
    }
}