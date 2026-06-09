using ChirayuHospitalMVC.Data;
using ChirayuHospitalMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChirayuHospitalMVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================
        // PATIENT DASHBOARD
        // ============================
        public async Task<IActionResult> Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");

            if (role != nameof(UserRole.Patient))
                return RedirectToAction("Dashboard", "Admin");

            int userId = int.Parse(userIdStr);

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
                return View();

            ViewBag.DoctorCount = await _context.Doctors.CountAsync();

            ViewBag.AppointmentCount = await _context.Appointments
                .CountAsync(a => a.PatientId == patient.PatientId);

            ViewBag.TodayAppointments = await _context.Appointments
                .CountAsync(a => a.PatientId == patient.PatientId &&
                                 a.Date.Date == DateTime.Today);

            return View();
        }


        // ============================
        // LIST PATIENTS
        // ============================
        public async Task<IActionResult> Index()
        {
            var patients = await _context.Patients
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            return View(patients);
        }


        // ============================
        // CREATE PATIENT
        // ============================
        public IActionResult Create()
        {
            LoadUsers();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient, string Username, string Password)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            // Check if username already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == Username);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Username already exists");
                return View(patient);
            }

            // Create User account
            var user = new User
            {
                Username = Username,
                Role = UserRole.Patient
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Create Patient profile
            patient.UserId = user.Id;
            patient.CreatedDate = DateTime.Now;

            _context.Patients.Add(patient);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // ============================
        // DETAILS
        // ============================
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }


        // ============================
        // EDIT
        // ============================
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient model)
        {
            if (id != model.PatientId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            patient.Name = model.Name;
            patient.Age = model.Age;
            patient.Gender = model.Gender;
            patient.Contact = model.Contact;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // ============================
        // DELETE
        // ============================
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient != null)
                _context.Patients.Remove(patient);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // ============================
        // LOAD USERS FOR DROPDOWN
        // ============================
        private void LoadUsers()
        {
            var users = _context.Users
                .Where(u => u.Role == UserRole.Patient &&
                            !_context.Patients.Any(p => p.UserId == u.Id))
                .Select(u => new
                {
                    u.Id,
                    u.Username
                })
                .ToList();

            ViewBag.Users = new SelectList(users, "Id", "Username");
        }
    }
}