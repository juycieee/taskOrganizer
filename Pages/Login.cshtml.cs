// TaskOrganizer.Pages.Login/Login.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using TaskOrganizer.Services;

namespace TaskOrganizer.Pages.Login
{
    public class LoginModel : PageModel
    {
        // Inject natin ang dalawang Services
        private readonly EmployeeService _employeeService;
        private readonly AdminService _adminService;

        public LoginModel(EmployeeService employeeService, AdminService adminService)
        {
            _employeeService = employeeService;
            _adminService = adminService;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new LoginInputModel();

        public void OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            // Tiyakin na valid ang lahat ng fields (dito na kukunin ang Email/Password errors)
            if (!ModelState.IsValid)
                return Page();

            var hashed = HashPassword(Input.Password);
            bool valid = false;

            if (Input.Role == "Admin")
            {
                // ADMIN LOGIN CHECK (Magche-check sa Admins collection)
                valid = await _adminService.ValidateLogin(Input.Email, hashed);

                if (valid)
                {
                    // Redirect sa Admin Dashboard
                    return RedirectToPage("/AdminDashboard");
                }
            }
            else // Employee Login Check
            {
                // EMPLOYEE LOGIN CHECK (Magche-check sa Employees collection)
                valid = await _employeeService.ValidateLogin(Input.Email, hashed);

                if (valid)
                {
                    // Redirect sa Employee Dashboard
                    return RedirectToPage("/Dashboard");
                }
            }

            // Kung hindi nag-match ang credentials o role
            ModelState.AddModelError("", "Invalid login attempt. Please check your credentials and selected account type.");
            return Page();
        }

        // Hashing function
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    // ✨ FIXED: Kumpletong LoginInputModel
    public class LoginInputModel
    {
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Oops! That doesn't look like a valid email.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Please select your account type.")]
        public string Role { get; set; } = "Employee";
    }
}