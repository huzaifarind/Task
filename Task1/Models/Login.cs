using System.ComponentModel.DataAnnotations;

namespace Task1.Models
{
    public class Login
    {
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string? UserEmail { get; set; } // Declare as nullable

        [Required]
        public string? UserPassword { get; set; } // Declare as nullable
    }
}
