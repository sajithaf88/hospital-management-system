using ChirayuHospitalMVC.Data;
using ChirayuHospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ChirayuHospitalMVC.Controllers
{
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================
        // LIST ALL BILLINGS
        // ============================
        public async Task<IActionResult> Index()
        {
            var bills = await _context.Billings
                .Include(b => b.Patient)
                .OrderByDescending(b => b.BillingDate)
                .ToListAsync();

            return View(bills);
        }

        // ============================
        // DETAILS
        // ============================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var bill = await _context.Billings
                .Include(b => b.Patient)
                .FirstOrDefaultAsync(b => b.BillingId == id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }

        // ============================
        // CREATE
        // ============================
        public IActionResult Create()
        {
            LoadPatients();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Billing billing)
        {
            if (!ModelState.IsValid)
            {
                LoadPatients(billing.PatientId);
                return View(billing);
            }

            billing.BillingDate = DateTime.Now;

            _context.Billings.Add(billing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // EDIT
        // ============================
        public async Task<IActionResult> Edit(int id)
        {
            var bill = await _context.Billings.FindAsync(id);

            if (bill == null)
                return NotFound();

            LoadPatients(bill.PatientId);

            return View(bill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Billing billing)
        {
            if (!ModelState.IsValid)
            {
                LoadPatients(billing.PatientId);
                return View(billing);
            }

            _context.Update(billing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // DELETE
        // ============================
        public async Task<IActionResult> Delete(int id)
        {
            var bill = await _context.Billings
                .Include(b => b.Patient)
                .FirstOrDefaultAsync(b => b.BillingId == id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bill = await _context.Billings.FindAsync(id);

            if (bill != null)
            {
                _context.Billings.Remove(bill);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // EXPORT BILLING CSV
        // ============================
        public async Task<IActionResult> ExportBilling()
        {
            var bills = await _context.Billings
                .Include(b => b.Patient)
                .OrderByDescending(b => b.BillingDate)
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Patient,Consultation,Medicine,Treatment,Other,Total,Date");

            foreach (var b in bills)
            {
                sb.AppendLine(
                    $"{b.Patient?.Name}," +
                    $"{b.ConsultationFee}," +
                    $"{b.MedicineCost}," +
                    $"{b.TreatmentCost}," +
                    $"{b.AdditionalCharges}," +
                    $"{b.TotalAmount}," +
                    $"{b.BillingDate:yyyy-MM-dd}"
                );
            }

            return File(
                Encoding.UTF8.GetBytes(sb.ToString()),
                "text/csv",
                $"Billing_{DateTime.Now:yyyyMMdd}.csv"
            );
        }

        // ============================
        // HELPER METHOD
        // ============================
        private void LoadPatients(int? selectedId = null)
        {
            ViewBag.Patients = new SelectList(
                _context.Patients.OrderBy(p => p.Name),
                "PatientId",
                "Name",
                selectedId
            );
        }
    }
}