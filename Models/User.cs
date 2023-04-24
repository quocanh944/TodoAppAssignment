using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAppAssignment.Models
{
    public class User
    {
        private string name;
        private string email;
        private string password;

        public User() {
        }

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get => name; set => name = value; }
        [Required]
        public string Email { get => email; set => email = value; }
        [Required]
        public string Password { get => password; set => password = value; }

        public List<TodoItem> TodoItems { get; set; }
    }
}
