using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Pages.Auth
{
    /// <summary>
    /// Handles authentication for both Admin and Employee users.
    /// Redirects to respective dashboards based on assigned role.
    /// </summary>
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;

        public LoginModel(
            SignInManager<Employee> signInManager,
            UserManager<Employee> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; } = new();

        /// <summary>
        /// GET: Redirect to dashboard if already logged in.
        /// </summary>
        public IActionResult OnGet()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToPage("/Admin/Dashboard");

            return Page();
        }

        /// <summary>
        /// POST: Validate credentials and redirect based on user role.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // ─── Attempt sign in ──────────────────────────────────────────
            var result = await _signInManager.PasswordSignInAsync(
                Input.Email,
                Input.Password,
                Input.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // ─── Redirect based on role ───────────────────────────────
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                    return RedirectToPage("/Admin/Dashboard");

                return RedirectToPage("/EmployeePortal/Profile");
            }

            TempData["Error"] = "Invalid email or password. Please try again.";
            return Page();
        }
    }
}