using EmployeeManagementSystem.Services.Interfaces;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Admin
{
    /// <summary>
    /// Handles creation of new employee accounts by Admin.
    /// Uses IEmployeeService to create user via ASP.NET Core Identity.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AddEmployeeModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        public AddEmployeeModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [BindProperty]
        public AddEmployeeViewModel Input { get; set; } = new();

        /// <summary>
        /// GET: Display the Add Employee form.
        /// </summary>
        public void OnGet() { }

        /// <summary>
        /// POST: Validate form and create new employee account.
        /// On success redirects to Employee list with success message.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var (success, errors) = await _employeeService.CreateEmployeeAsync(Input);

            if (!success)
            {
                TempData["Error"] = string.Join(" ", errors);
                return Page();
            }

            TempData["Success"] = $"Employee {Input.FirstName} {Input.LastName} created successfully.";
            return RedirectToPage("/Admin/Employees");
        }
    }
}