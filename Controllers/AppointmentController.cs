using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChirayuHospitalMVC.Data;
using ChirayuHospitalMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace ChirayuHospitalMVC.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================
        // ADMIN / STAFF : ALL APPOINTMENTS
        // ============================
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != nameof(UserRole.Admin) && role != nameof(UserRole.Staff))
                return RedirectToAction("Login", "Account");

            var list = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return View(list);
        }

        // ============================
        // PATIENT : MY APPOINTMENTS
        // ============================
        public async Task<IActionResult> MyAppointments()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            var role = HttpContext.Session.GetString("UserRole");

            if (role != nameof(UserRole.Patient) || string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdStr);

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
                return View(Enumerable.Empty<Appointment>());

            var list = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patient.PatientId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return View(list);
        }

        // ============================
        // CREATE APPOINTMENT
        // ============================
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != nameof(UserRole.Patient))
                return RedirectToAction("Login", "Account");

            LoadDoctors();
            return View();
        }

        // ============================
        // CREATE POST
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (role != nameof(UserRole.Patient) || string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                LoadDoctors();
                return View(appointment);
            }

            int userId = int.Parse(userIdStr);

            // Find patient profile
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            // If patient does not exist create automatically
            if (patient == null)
            {
                var user = await _context.Users.FindAsync(userId);

                patient = new Patient
                {
                    Name = user.Username,
                    UserId = userId
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            appointment.PatientId = patient.PatientId;
            appointment.Status = AppointmentStatus.Scheduled;

            // Generate Token Number
            var todayCount = await _context.Appointments
                .CountAsync(a => a.Date.Date == appointment.Date.Date);

            appointment.TokenNumber = $"T{todayCount + 1:000}";

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyAppointments));
        }

        // ============================
        // APPROVE (MARK COMPLETED)
        // ============================
        public async Task<IActionResult> Approve(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != nameof(UserRole.Admin) && role != nameof(UserRole.Staff))
                return RedirectToAction("Login", "Account");

            var appt = await _context.Appointments.FindAsync(id);

            if (appt == null)
                return NotFound();

            appt.Status = AppointmentStatus.Completed;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // CANCEL
        // ============================
        public async Task<IActionResult> Cancel(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);

            if (appt == null)
                return NotFound();

            appt.Status = AppointmentStatus.Cancelled;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // API : GET PATIENT + DOCTOR
        // ============================
        [HttpGet]
        public async Task<IActionResult> GetAppointmentInfo(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound();

            return Json(new
            {
                patientId = appointment.PatientId,
                doctorId = appointment.DoctorId
            });
        }

        // ============================
        // HELPER : LOAD DOCTORS
        // ============================
        private void LoadDoctors()
        {
            ViewBag.Doctors = new SelectList(
                _context.Doctors.OrderBy(d => d.Name).ToList(),
                "DoctorId",
                "Name"
            );
        }
    }
}