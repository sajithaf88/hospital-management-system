using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ChirayuHospitalMVC.Data;
using ChirayuHospitalMVC.Models;

namespace ChirayuHospitalMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;


    public AccountController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // ==========================  
        // LOGIN (GET)  
        // ==========================  
        [HttpGet]
        public IActionResult Login()
        {
            if (IsLoggedIn())
                return RedirectBasedOnRole();

            return View();
        }

        // ==========================  
        // LOGIN (POST)  
        // ==========================  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                model.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            SetUserSession(user);

            return RedirectBasedOnRole();
        }

        // ==========================  
        // REGISTER (GET)  
        // ==========================  
        [HttpGet]
        public IActionResult Register()
        {
            if (IsLoggedIn())
                return RedirectBasedOnRole();

            return View();
        }

        // ==========================  
        // REGISTER (POST)  
        // ==========================  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Username check  
            if (await _context.Users.AnyAsync(u => u.Username == vm.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(vm);
            }

            // Email check  
            if (!string.IsNullOrEmpty(vm.Email))
            {
                if (await _context.Users.AnyAsync(u => u.Email == vm.Email))
                {
                    ModelState.AddModelError("Email", "Email already registered");
                    return View(vm);
                }
            }

            // ======================  
            // CREATE USER  
            // ======================  
            var user = new User
            {
                Username = vm.Username,
                Email = vm.Email,
                Role = UserRole.Patient
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, vm.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // ======================  
            // CREATE PATIENT PROFILE  
            // ======================  

            bool patientExists = await _context.Patients
                .AnyAsync(p => p.UserId == user.Id);

            if (!patientExists)
            {
                var patient = new Patient
                {
                    UserId = user.Id,
                    Name = vm.Username,
                    CreatedDate = DateTime.Now
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Account created successfully. Please login.";

            return RedirectToAction("Login");
        }

        // ==========================  
        // PROFILE  
        // ==========================  
        public async Task<IActionResult> Profile()
        {
            var user = await GetCurrentUser();

            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        // ==========================  
        // CHANGE PASSWORD  
        // ==========================  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var user = await GetCurrentUser();

            if (user == null)
                return RedirectToAction("Login");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                currentPassword
            );

            if (result == PasswordVerificationResult.Failed)
            {
                TempData["Error"] = "Current password is incorrect";
                return RedirectToAction("Profile");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Password changed successfully";

            return RedirectToAction("Profile");
        }

        // ==========================  
        // LOGOUT  
        // ==========================  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ==========================  
        // HELPER METHODS  
        // ==========================  

        private bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
        }

        private void SetUserSession(User user)
        {
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());
        }

        private async Task<User?> GetCurrentUser()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdStr))
                return null;

            if (!int.TryParse(userIdStr, out int userId))
                return null;

            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        // ==========================  
        // ROLE REDIRECTION  
        // ==========================  
        private IActionResult RedirectBasedOnRole()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role == nameof(UserRole.Admin))
                return RedirectToAction("Dashboard", "Admin");

            if (role == nameof(UserRole.Staff))
                return RedirectToAction("Dashboard", "Admin");

            if (role == nameof(UserRole.Patient))
                return RedirectToAction("Dashboard", "Patient");

            return RedirectToAction("Login");
        }
    }  


}
