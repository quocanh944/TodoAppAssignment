using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoAppAssignment.Models;

namespace TodoAppAssignment.Data
{
    public class TodoAppAssignmentContext : DbContext
    {
        public TodoAppAssignmentContext (DbContextOptions<TodoAppAssignmentContext> options)
            : base(options)
        {
        }

        public DbSet<TodoAppAssignment.Models.User> User { get; set; } = default!;

        public DbSet<TodoAppAssignment.Models.TodoItem>? TodoItem { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
