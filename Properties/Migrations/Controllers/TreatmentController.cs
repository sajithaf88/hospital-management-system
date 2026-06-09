using ChirayuHospitalMVC.Data;
using ChirayuHospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChirayuHospitalMVC.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreatmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================
        // LIST
        // ======================
        public async Task<IActionResult> Index()
        {
            var treatments = await _context.Treatments
                .Include(t => t.Patient)
                .Include(t => t.Doctor)
                .Include(t => t.Appointment)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();

            return View(treatments);
        }

        // ======================
        // CREATE
        // ======================
        public IActionResult Create()
        {
            LoadDropdowns();

            var treatment = new Treatment
            {
                Medicines = new List<MedicineEntry>
                {
                    new MedicineEntry()
                }
            };

            return View(treatment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Treatment treatment)
        {
            if (!ModelState.IsValid)
            {
                if (treatment.Medicines == null || treatment.Medicines.Count == 0)
                    treatment.Medicines = new List<MedicineEntry> { new MedicineEntry() };

                LoadDropdowns();
                return View(treatment);
            }

            treatment.CreatedDate = DateTime.Now;

            _context.Treatments.Add(treatment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ======================
        // EDIT
        // ======================
        public async Task<IActionResult> Edit(int id)
        {
            var treatment = await _context.Treatments
                .Include(t => t.Medicines)
                .FirstOrDefaultAsync(t => t.TreatmentId == id);

            if (treatment == null)
                return NotFound();

            LoadDropdowns(treatment);

            return View(treatment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Treatment treatment)
        {
            if (id != treatment.TreatmentId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                LoadDropdowns(treatment);
                return View(treatment);
            }

            _context.Update(treatment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ======================
        // DETAILS
        // ======================
        public async Task<IActionResult> Details(int id)
        {
            var treatment = await _context.Treatments
                .Include(t => t.Patient)
                .Include(t => t.Doctor)
                .Include(t => t.Medicines)
                .FirstOrDefaultAsync(t => t.TreatmentId == id);

            if (treatment == null)
                return NotFound();

            return View(treatment);
        }

        // ======================
        // DELETE
        // ======================
        public async Task<IActionResult> Delete(int id)
        {
            var treatment = await _context.Treatments
                .Include(t => t.Patient)
                .FirstOrDefaultAsync(t => t.TreatmentId == id);

            if (treatment == null)
                return NotFound();

            return View(treatment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatment = await _context.Treatments.FindAsync(id);

            if (treatment != null)
            {
                _context.Treatments.Remove(treatment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ======================
        // PRESCRIPTION VIEW
        // ======================
        public async Task<IActionResult> Prescription(int id)
        {
            var treatment = await _context.Treatments
                .Include(t => t.Patient)
                .Include(t => t.Doctor)
                .Include(t => t.Appointment)
                .Include(t => t.Medicines)
                .FirstOrDefaultAsync(t => t.TreatmentId == id);

            if (treatment == null)
                return NotFound();

            return View(treatment);
        }

        // ======================
        // LOAD DROPDOWNS
        // ======================
        private void LoadDropdowns(Treatment? treatment = null)
        {
            ViewBag.Patients = new SelectList(
                _context.Patients,
                "PatientId",
                "Name",
                treatment?.PatientId
            );

            ViewBag.Doctors = new SelectList(
                _context.Doctors,
                "DoctorId",
                "Name",
                treatment?.DoctorId
            );

            ViewBag.Appointments = new SelectList(
                _context.Appointments,
                "AppointmentId",
                "TokenNumber",
                treatment?.AppointmentId
            );

            ViewBag.Medicines = new SelectList(
                _context.Medicines,
                "Name",
                "Name"
            );
        }
    }
}