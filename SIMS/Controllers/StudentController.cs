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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(User model)
        {
            // Remove validation for fields handled by the service layer
            ModelState.Remove("HashPassword");
            ModelState.Remove("Role");
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                await _userService.AddStudentAsync(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return NotFound();

            var student = await _userService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User model)
        {
            if (id != model.Id) return BadRequest();

            // Ignore fields that shouldn't be updated or are handled internally
            ModelState.Remove("HashPassword");
            ModelState.Remove("Role");
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                var success = await _userService.UpdateStudentAsync(model);
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Unable to save changes. The student may no longer exist.");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return NotFound();

            var student = await _userService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _userService.DeleteStudentAsync(id);
            if (!success)
            {
                // Handle error if necessary
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}