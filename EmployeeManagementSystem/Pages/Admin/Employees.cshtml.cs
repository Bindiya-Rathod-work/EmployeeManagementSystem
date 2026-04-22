using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Admin
{
    /// <summary>
    /// Displays a paginated, searchable list of all active employees.
    /// Supports soft delete directly from the list view.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class EmployeesModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();

        /// <summary>
        /// GET: Load all active employees.
        /// </summary>
        public async Task OnGetAsync()
        {
            Employees = await _employeeService.GetAllEmployeesAsync();
        }

        /// <summary>
        /// POST: Soft delete an employee by ID.
        /// </summary>
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Invalid employee ID.";
                return RedirectToPage();
            }

            var result = await _employeeService.SoftDeleteEmployeeAsync(id);

            TempData[result ? "Success" : "Error"] = result
                ? "Employee deleted successfully."
                : "Employee not found or already deleted.";

            return RedirectToPage();
        }
    }
}