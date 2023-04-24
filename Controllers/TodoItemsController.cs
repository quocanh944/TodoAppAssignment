using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoAppAssignment.Data;
using TodoAppAssignment.Models;

namespace TodoAppAssignment.Controllers
{
    [Authorize]
    public class TodoItemsController : Controller
    {
        private readonly TodoAppAssignmentContext _context;

        public TodoItemsController(TodoAppAssignmentContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Content")] TodoItem todoItem)
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == email);
            todoItem.UserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.Add(todoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id")] int id)
        {
            Console.WriteLine("Id");
            Console.WriteLine(id);
            if (_context.TodoItem == null)
            {
                return Problem("Entity set 'TodoAppAssignmentContext.TodoItem'  is null.");
            }
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return Problem("Wrong id.");
            }

            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == email);

            if (!user.Id.Equals(todoItem.UserId))
            {
                return Problem("Not your todo item");
            }
    
            _context.TodoItem.Remove(todoItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        private bool TodoItemExists(int id)
        {
          return (_context.TodoItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
