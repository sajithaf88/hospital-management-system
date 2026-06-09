using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChirayuHospitalMVC.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Specialization { get; set; }

        [Phone]
        public string? Contact { get; set; }

        public int? DepartmentId { get; set; }

        public Department? Department { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}