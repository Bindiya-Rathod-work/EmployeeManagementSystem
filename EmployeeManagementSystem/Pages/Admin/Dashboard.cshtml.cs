using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Admin
{
    /// <summary>
    /// Admin Dashboard page model.
    /// Displays summary statistics and recent employee activity.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDocumentService _documentService;

        public DashboardModel(
            IEmployeeService employeeService,
            IDocumentService documentService)
        {
            _employeeService = employeeService;
            _documentService = documentService;
        }

        public int TotalEmployees { get; set; }
        public int NewThisMonth { get; set; }
        public int TotalDocuments { get; set; }
        public IEnumerable<Employee> RecentEmployees { get; set; } = new List<Employee>();

        /// <summary>
        /// GET: Load dashboard statistics and recent employee list.
        /// </summary>
        public async Task OnGetAsync()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            var employeeList = employees.ToList();

            // ─── Calculate stats ──────────────────────────────────────────
            TotalEmployees = employeeList.Count;

            NewThisMonth = employeeList.Count(e =>
                e.DateOfJoining.Month == DateTime.UtcNow.Month &&
                e.DateOfJoining.Year == DateTime.UtcNow.Year);

            TotalDocuments = employeeList.Sum(e => e.Documents.Count);

            // ─── Get 5 most recent employees ──────────────────────────────
            RecentEmployees = employeeList.Take(5);
        }
    }
}