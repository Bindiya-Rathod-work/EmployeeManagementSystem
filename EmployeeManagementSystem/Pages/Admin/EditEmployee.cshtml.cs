using EmployeeManagementSystem.Services.Interfaces;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Admin
{
    /// <summary>
    /// Handles editing of an existing employee's profile by Admin.
    /// Loads current data into form on GET, saves changes on POST.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class EditEmployeeModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        public EditEmployeeModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [BindProperty]
        public EditEmployeeViewModel Input { get; set; } = new();

        /// <summary>
        /// GET: Load existing employee data into the edit form.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToPage("/Admin/Employees");

            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                TempData["Error"] = "Employee not found.";
                return RedirectToPage("/Admin/Employees");
            }

            // ─── Map entity to ViewModel ──────────────────────────────────
            Input = new EditEmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email ?? string.Empty,
                Department = employee.Department,
                Designation = employee.Designation,
                DateOfJoining = employee.DateOfJoining,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address
            };

            return Page();
        }

        /// <summary>
        /// POST: Validate and save updated employee details.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var (success, errors) = await _employeeService.UpdateEmployeeAsync(Input);

            if (!success)
            {
                TempData["Error"] = string.Join(" ", errors);
                return Page();
            }

            TempData["Success"] = "Employee details updated successfully.";
            return RedirectToPage("/Admin/Employees");
        }
    }
}