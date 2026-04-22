using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Employee = EmployeeManagementSystem.Models.Employee;

namespace EmployeeManagementSystem.Pages.Admin
{
    /// <summary>
    /// Displays paginated list of employees with Active/Inactive filter.
    /// Supports soft delete and reactivation directly from the list.
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
        public string Filter { get; set; } = "active";
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }

        /// <summary>
        /// GET: Load employees based on active/inactive filter.
        /// </summary>
        public async Task OnGetAsync(string filter = "active")
        {
            Filter = filter;

            // ─── Load counts for both tabs ────────────────────────────────
            var active = await _employeeService.GetAllEmployeesAsync();
            var inactive = await _employeeService.GetInactiveEmployeesAsync();

            ActiveCount = active.Count();
            InactiveCount = inactive.Count();

            // ─── Load correct list based on filter ────────────────────────
            Employees = filter == "inactive" ? inactive : active;
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
                ? "Employee deactivated successfully."
                : "Employee not found.";

            return RedirectToPage(new { filter = "active" });
        }

        /// <summary>
        /// POST: Reactivate a soft deleted employee by ID.
        /// </summary>
        public async Task<IActionResult> OnPostReactivateAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Invalid employee ID.";
                return RedirectToPage();
            }

            var result = await _employeeService.ReactivateEmployeeAsync(id);

            TempData[result ? "Success" : "Error"] = result
                ? "Employee reactivated successfully."
                : "Employee not found.";

            return RedirectToPage(new { filter = "active" });
        }
    }
}