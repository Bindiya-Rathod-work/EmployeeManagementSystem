using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Pages.Auth
{
    /// <summary>
    /// Handles user logout and redirects to login page.
    /// </summary>
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Employee> _signInManager;

        public LogoutModel(SignInManager<Employee> signInManager)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// POST: Signs out the current user and redirects to login.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Auth/Login");
        }
    }
}