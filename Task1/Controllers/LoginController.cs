using Microsoft.AspNetCore.Mvc;
using Task1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Task1.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly UserDbContext _dbContext;

        public LoginController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Register() => View(new Register());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Register user)
        {
            if (!ModelState.IsValid)
                return View(user);

            int isSuccessful = _dbContext.ExecuteUserManagementProcedure(
                operationId: 1,
                out int loginAttemptId,
                out int userId,
                userName: user.UserName,
                userEmail: user.UserEmail,
                userPassword: user.UserPassword,
                isActive: true);

            if (isSuccessful == 1)
                return RedirectToAction("Login");

            ViewBag.ErrorMessage = "Registration failed. Please try again.";
            return View(user);
        }

        public IActionResult Login() => View(new Login());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int isSuccessful = _dbContext.ExecuteUserManagementProcedure(
                operationId: 2,
                out int loginAttemptId,
                out int userId,
                userEmail: model.UserEmail,
                userPassword: model.UserPassword,
                userName: null,
                isActive: null);

            if (isSuccessful == 1)
            {
                // Authentication successful, create cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, model.UserEmail)
                    // Add more claims as needed
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    // Customize properties if needed
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid email or password. Please try again.";
            return View(model);
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
