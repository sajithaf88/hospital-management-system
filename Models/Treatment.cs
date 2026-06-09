using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChirayuHospitalMVC.Models
{
    public class Treatment
    {
        [Key]
        public int TreatmentId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment? Appointment { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }

        [StringLength(500)]
        public string Complaint { get; set; } = "";

        [StringLength(500)]
        public string Diagnosis { get; set; } = "";

        public List<MedicineEntry> Medicines { get; set; } = new();

        public int? BP_Systolic { get; set; }

        public int? BP_Diastolic { get; set; }

        public int? RespirationRate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Temperature { get; set; }

        [StringLength(500)]
        public string Advice { get; set; } = "";

        [StringLength(1000)]
        public string Notes { get; set; } = "";

        public DateTime? NextVisit { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}