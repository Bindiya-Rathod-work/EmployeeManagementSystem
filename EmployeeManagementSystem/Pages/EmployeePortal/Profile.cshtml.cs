using EmployeeManagementSystem.Services.Interfaces;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Pages.EmployeePortal
{
    /// <summary>
    /// Displays the logged-in employee's basic profile information.
    /// Employees can only view their own profile — no edit access.
    /// </summary>
    [Authorize(Roles = "Employee")]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IEmployeeService _employeeService;

        public ProfileModel(
            UserManager<Employee> userManager,
            IEmployeeService employeeService)
        {
            _userManager = userManager;
            _employeeService = employeeService;
        }

        public EmployeeProfileViewModel Profile { get; set; } = new();

        /// <summary>
        /// GET: Load the currently logged-in employee's profile data.
        /// Redirects to login if user is not found.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            // ─── Get currently logged in user ─────────────────────────────
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Auth/Login");

            // ─── Map to ViewModel — only expose basic info ────────────────
            Profile = new EmployeeProfileViewModel
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email ?? string.Empty,
                Department = user.Department,
                Designation = user.Designation,
                DateOfJoining = user.DateOfJoining,
                PhoneNumber = user.PhoneNumber
            };

            return Page();
        }
    }
}