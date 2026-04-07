using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models;
using SIMS.Services.Interfaces;
using System.Security.Claims;

namespace SIMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        public LoginController(IUserService service)
        {
            _userService = service;
        }
        public IActionResult Index()
        {
            return View(); // load view
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string username = model.Username.ToString().Trim();
                string password = model.Password.ToString().Trim();
                var user = await _userService.LoginUserAsync(username, password);

                if (user != null)
                {
                    //ma hoa thong tin user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity)
                    );

                    //successful - forward to dashboard controller
                    return RedirectToAction("Index", "Dashboard");
                }
                ViewData["InvalidLogin"] = "Username or Password invalid";
            }
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
