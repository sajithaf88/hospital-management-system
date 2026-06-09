using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChirayuHospitalMVC.Models
{
    public enum GenderType
    {
        Male,
        Female,
        Other
    }

    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 120)]
        public int Age { get; set; }

        public GenderType? Gender { get; set; }

        [Phone]
        public string? Contact { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // 🔗 Relationships
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<Billing> Billings { get; set; } = new List<Billing>();
    }
}