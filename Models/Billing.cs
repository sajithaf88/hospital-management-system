using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChirayuHospitalMVC.Models
{
    public class Billing
    {
        [Key]
        public int BillingId { get; set; }

        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ConsultationFee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MedicineCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TreatmentCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionalCharges { get; set; }

        // Auto-calculated
        [NotMapped]
        public decimal TotalAmount =>
            ConsultationFee +
            MedicineCost +
            TreatmentCost +
            AdditionalCharges;

        public DateTime BillingDate { get; set; } = DateTime.Now;
    }
}