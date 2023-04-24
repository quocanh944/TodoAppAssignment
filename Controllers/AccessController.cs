using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TodoAppAssignment.Models;
using TodoAppAssignment.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoAppAssignment.Controllers
{
    public class AccessController : Controller
    {
        private readonly TodoAppAssignmentContext _context;

        public AccessController(TodoAppAssignmentContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel modelLogin)
        {
            
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Email == modelLogin.Email && m.Password == modelLogin.Password);
            if (user == null)
            {
                ViewData["ValidateMessage"] = "Wrong Username or Password";

                return View();
            }

            List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, user.Name),
                    new Claim(ClaimTypes.Email, modelLogin.Email)
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
