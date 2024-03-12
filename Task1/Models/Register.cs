using System.ComponentModel.DataAnnotations;

namespace Task1.Models
{
    public class Register
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public string UserPassword { get; set; }

        public bool IsActive { get; set; }
    }
}
