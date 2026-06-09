using ChirayuHospitalMVC.Data;
using ChirayuHospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChirayuHospitalMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(role))
                return RedirectToAction("Login", "Account");

            if (role != nameof(UserRole.Admin) && role != nameof(UserRole.Staff))
                return RedirectToAction("Index", "Home");

            ViewBag.TotalPatients = _context.Patients.Count();
            ViewBag.TotalDoctors = _context.Doctors.Count();
            ViewBag.TotalAppointments = _context.Appointments.Count();

            var scheduled = _context.Appointments.Count(a => a.Status == AppointmentStatus.Scheduled);
            var completed = _context.Appointments.Count(a => a.Status == AppointmentStatus.Completed);
            var cancelled = _context.Appointments.Count(a => a.Status == AppointmentStatus.Cancelled);

            ViewBag.ScheduledAppointments = scheduled;
            ViewBag.CompletedAppointments = completed;
            ViewBag.CancelledAppointments = cancelled;

            ViewBag.TotalRevenue = _context.Billings
                .Sum(b => (decimal?)(
                    b.ConsultationFee +
                    b.MedicineCost +
                    b.TreatmentCost +
                    b.AdditionalCharges
                )) ?? 0;

            var start = DateTime.Today;
            var end = start.AddDays(1);

            ViewBag.TodayAppointments = _context.Appointments
                .Count(a => a.Date >= start && a.Date < end);

            ViewBag.TodayPatients = _context.Patients
                .Count(p => p.CreatedDate >= start && p.CreatedDate < end);

            ViewBag.TodayRevenue = _context.Billings
                .Where(b => b.BillingDate >= start && b.BillingDate < end)
                .Sum(b => (decimal?)(
                    b.ConsultationFee +
                    b.MedicineCost +
                    b.TreatmentCost +
                    b.AdditionalCharges
                )) ?? 0;

            var days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-6 + i))
                .ToList();

            var chartLabels = days.Select(d => d.ToString("dd MMM")).ToList();

            var chartData = days.Select(d =>
            {
                var next = d.AddDays(1);
                return _context.Appointments.Count(a => a.Date >= d && a.Date < next);
            }).ToList();

            ViewBag.ChartLabels = chartLabels;
            ViewBag.ChartData = chartData;

            ViewBag.StatusData = new[] { scheduled, completed, cancelled };

            ViewBag.RecentAppointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.Date)
                .Take(5)
                .ToList();

            ViewBag.DoctorPerformance = _context.Appointments
                .Include(a => a.Doctor)
                .GroupBy(a => a.Doctor.Name)
                .Select(g => new
                {
                    Doctor = g.Key,
                    Patients = g.Count()
                })
                .OrderByDescending(x => x.Patients)
                .Take(5)
                .ToList();

            return View();
        }
    }
}