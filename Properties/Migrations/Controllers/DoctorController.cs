using ChirayuHospitalMVC.Data;
using ChirayuHospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChirayuHospitalMVC.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================
        // LIST ALL DOCTORS
        // ============================
        public async Task<IActionResult> Index()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Department)
                .OrderBy(d => d.Name)
                .ToListAsync();

            return View(doctors);
        }

        // ============================
        // DETAILS
        // ============================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        // ============================
        // CREATE
        // ============================
        public IActionResult Create()
        {
            LoadDepartments();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                LoadDepartments();
                return View(doctor);
            }

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // EDIT
        // ============================
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
                return NotFound();

            LoadDepartments();
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                LoadDepartments();
                return View(doctor);
            }

            try
            {
                _context.Update(doctor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(doctor.DoctorId))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // DELETE
        // ============================
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null)
                return NotFound();

            // Prevent delete if appointments exist
            if (doctor.Appointments.Any())
            {
                TempData["Error"] = "Cannot delete doctor with existing appointments.";
                return RedirectToAction(nameof(Index));
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // LOAD DEPARTMENTS DROPDOWN
        // ============================
        private void LoadDepartments()
        {
            var departments = _context.Departments
                .OrderBy(d => d.Name)
                .Select(d => new
                {
                    d.DepartmentId,
                    d.Name
                })
                .ToList();

            ViewBag.Departments = new SelectList(departments, "DepartmentId", "Name");
        }

        // ============================
        // CHECK EXISTENCE
        // ============================
        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(d => d.DoctorId == id);
        }
    }
}