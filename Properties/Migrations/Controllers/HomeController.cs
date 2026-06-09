using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ChirayuHospitalMVC.Models;

namespace ChirayuHospitalMVC.Controllers
{
    public class HomeController : Controller
    {
        // ============================
        // PUBLIC LANDING PAGE
        // ============================
        public IActionResult Landing()
        {
            return View(); // Views/Home/Landing.cshtml
        }

        // ============================
        // ROLE BASED HOME REDIRECT
        // ============================
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(role))
                return RedirectToAction(nameof(Landing));

            if (role == nameof(UserRole.Patient))
                return RedirectToAction("Dashboard", "Patient");

            if (role == nameof(UserRole.Admin) || role == nameof(UserRole.Staff))
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction(nameof(Landing));
        }

        // ============================
        // ACCESS DENIED PAGE
        // ============================
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}