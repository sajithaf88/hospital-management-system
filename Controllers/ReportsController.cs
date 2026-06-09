using ChirayuHospitalMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ChirayuHospitalMVC.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================
        // REPORT DASHBOARD
        // ============================
        public IActionResult Index()
        {
            return View();
        }

        // ============================
        // BILLING REPORT
        // ============================
        public async Task<IActionResult> BillingReport(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Billings
                .Include(b => b.Patient)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(b => b.BillingDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(b => b.BillingDate <= endDate.Value);

            var invoices = await query
                .OrderByDescending(b => b.BillingDate)
                .ToListAsync();

            ViewBag.TotalRevenue = invoices.Sum(b => b.TotalAmount);

            return View(invoices);
        }

        // ============================
        // PATIENTS REPORT
        // ============================
        public async Task<IActionResult> PatientsReport()
        {
            var patients = await _context.Patients
                .OrderBy(p => p.Name)
                .ToListAsync();

            ViewBag.TotalPatients = patients.Count;

            return View(patients);
        }

        // ============================
        // EXPORT BILLING CSV
        // ============================
        public async Task<IActionResult> ExportBilling()
        {
            var invoices = await _context.Billings
                .Include(b => b.Patient)
                .OrderByDescending(b => b.BillingDate)
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("InvoiceId,Patient,TotalAmount,BillingDate");

            foreach (var b in invoices)
            {
                sb.AppendLine(
                    $"{b.BillingId}," +
                    $"\"{b.Patient?.Name}\"," +
                    $"{b.TotalAmount:F2}," +
                    $"{b.BillingDate:yyyy-MM-dd}"
                );
            }

            return File(
                Encoding.UTF8.GetBytes(sb.ToString()),
                "text/csv",
                $"BillingReport_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            );
        }

        // ============================
        // EXPORT PATIENTS CSV
        // ============================
        public async Task<IActionResult> ExportPatients()
        {
            var list = await _context.Patients
                .OrderBy(p => p.Name)
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Id,Name,Age,Gender,Contact,Registered");

            foreach (var p in list)
            {
                sb.AppendLine(
                    $"{p.PatientId}," +
                    $"\"{p.Name}\"," +
                    $"{p.Age}," +
                    $"{p.Gender}," +
                    $"{p.Contact}," +
                    $"{p.CreatedDate:yyyy-MM-dd}"
                );
            }

            return File(
                Encoding.UTF8.GetBytes(sb.ToString()),
                "text/csv",
                $"Patients_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            );
        }
    }
}