using ChirayuHospitalMVC.Data;
using ChirayuHospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChirayuHospitalMVC.Controllers
{
    public class MedicineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================
        // LIST ALL MEDICINES
        // ============================
        public async Task<IActionResult> Index()
        {
            var medicines = await _context.Medicines
                .OrderBy(m => m.Name)
                .ToListAsync();

            return View(medicines);
        }

        // ============================
        // DETAILS
        // ============================
        public async Task<IActionResult> Details(int id)
        {
            var medicine = await _context.Medicines
                .FirstOrDefaultAsync(m => m.MedicineId == id);

            if (medicine == null)
                return NotFound();

            return View(medicine);
        }

        // ============================
        // CREATE
        // ============================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medicine medicine)
        {
            if (!ModelState.IsValid)
                return View(medicine);

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // EDIT
        // ============================
        public async Task<IActionResult> Edit(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
                return NotFound();

            return View(medicine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Medicine medicine)
        {
            if (id != medicine.MedicineId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(medicine);

            try
            {
                _context.Update(medicine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(medicine.MedicineId))
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
            var medicine = await _context.Medicines
                .FirstOrDefaultAsync(m => m.MedicineId == id);

            if (medicine == null)
                return NotFound();

            return View(medicine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine != null)
            {
                _context.Medicines.Remove(medicine);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // CHECK EXISTENCE
        // ============================
        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.MedicineId == id);
        }
    }
}