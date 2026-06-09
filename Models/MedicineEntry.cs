using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChirayuHospitalMVC.Models
{
    public enum MedicineTiming
    {
        Morning,
        Afternoon,
        Evening,
        Night
    }

    public class MedicineEntry
    {
        [Key]
        public int MedicineEntryId { get; set; }

        public int TreatmentId { get; set; }

        [ForeignKey("TreatmentId")]
        public Treatment? Treatment { get; set; }

        [Required]
        [StringLength(200)]
        public string Medicine { get; set; } = "";

        [Range(1, 30)]
        public int Days { get; set; }

        public MedicineTiming WhenToTake { get; set; }

        public bool BeforeMeal { get; set; }
    }
}