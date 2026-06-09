using Microsoft.EntityFrameworkCore;
using ChirayuHospitalMVC.Models;

namespace ChirayuHospitalMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<MedicineEntry> MedicineEntries { get; set; }
        public DbSet<Billing> Billings { get; set; }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ========================
            // USER → PATIENT (1 : 1)
            // ========================
            builder.Entity<Patient>()
                .HasOne(p => p.User)
                .WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================
            // APPOINTMENT RELATIONSHIPS
            // ========================
            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================
            // TREATMENT RELATIONSHIPS
            // ========================
            builder.Entity<Treatment>()
                .HasOne(t => t.Patient)
                .WithMany()
                .HasForeignKey(t => t.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Treatment>()
                .HasOne(t => t.Doctor)
                .WithMany()
                .HasForeignKey(t => t.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Treatment>()
                .HasOne(t => t.Appointment)
                .WithMany()
                .HasForeignKey(t => t.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================
            // MEDICINE ENTRY
            // ========================
            builder.Entity<MedicineEntry>()
                .HasOne(m => m.Treatment)
                .WithMany(t => t.Medicines)
                .HasForeignKey(m => m.TreatmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========================
            // BILLING
            // ========================
            builder.Entity<Billing>()
                .HasOne(b => b.Patient)
                .WithMany(p => p.Billings)
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}