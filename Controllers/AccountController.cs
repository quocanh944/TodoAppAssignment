using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoAppAssignment.Models;
using TodoAppAssignment.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoAppAssignment.Controllers
{
    public class AccountController : Controller
    {
        private readonly TodoAppAssignmentContext _context;

        public AccountController(TodoAppAssignmentContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel modelRegister)
        {
            if (modelRegister.Name == null) {
                ViewData["ValidateMessage"] = "Name cannot be blank.";

                return View();
            }

            if (modelRegister.Email == null) {
                ViewData["ValidateMessage"] = "Email cannot be blank.";

                return View();
            }

            if (modelRegister.Password == null)
            {
                ViewData["ValidateMessage"] = "Email cannot be blank.";

                return View();
            }

            if (modelRegister.Password.Length < 7)
            {
                ViewData["ValidateMessage"] = "Password length cannot be less than 8.";

                return View();
            }

            if (modelRegister.Password != modelRegister.ConfirmPassword)
            {
                ViewData["ValidateMessage"] = "Password and Confirm Password not match.";

                return View();
            }

           
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Email == modelRegister.Email);

            if (user != null)
            {
                ViewData["ValidateMessage"] = "This email already exists.";

                return View();
            }

            _context.Add(new User(modelRegister.Name, modelRegister.Email, modelRegister.Password));
            await _context.SaveChangesAsync();

            List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, modelRegister.Name),
                    new Claim(ClaimTypes.Email, modelRegister.Email)
                };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity), properties);

            return RedirectToAction("Index", "Home");
        }
    }
}
