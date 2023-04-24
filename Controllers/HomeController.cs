using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoAppAssignment.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using NuGet.Protocol;
using TodoAppAssignment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace TodoAppAssignment.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly TodoAppAssignmentContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, TodoAppAssignmentContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _context.User.Include(x => x.TodoItems).SingleOrDefaultAsync(m => m.Email == email);

            var viewModel = new HomeViewModel();
            viewModel.User = user;

            if (user != null)
            {
                return View(viewModel);
            }

            return RedirectToAction("Access", "Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Access");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}