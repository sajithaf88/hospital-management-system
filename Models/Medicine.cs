using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChirayuHospitalMVC.Models
{
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // Tablet / Syrup / Injection / Ointment etc.
        [Required]
        public string Type { get; set; } = string.Empty;

        // Stock Quantity
        [Required]
        public int Quantity { get; set; }

        // Price per unit
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
