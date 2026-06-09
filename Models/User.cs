using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ChirayuHospitalMVC.Models
{
    public enum UserRole
    {
        Admin,
        Staff,
        Patient
    }

    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        // 🔗 Navigation property (1 User → 1 Patient profile)
        public Patient? Patient { get; set; }
    }
}