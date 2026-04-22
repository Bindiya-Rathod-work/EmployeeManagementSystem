using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Admin
{
    /// <summary>
    /// Displays full employee profile and manages their PDF documents.
    /// Supports document upload, view, and soft delete operations.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class EmployeeDetailsModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDocumentService _documentService;

        public EmployeeDetailsModel(
            IEmployeeService employeeService,
            IDocumentService documentService)
        {
            _employeeService = employeeService;
            _documentService = documentService;
        }

        public Employee Employee { get; set; } = null!;
        public IEnumerable<EmployeeDocument> Documents { get; set; } = new List<EmployeeDocument>();

        /// <summary>
        /// GET: Load employee details and their documents.
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

            Employee = employee;
            Documents = await _documentService.GetDocumentsByEmployeeIdAsync(id);
            return Page();
        }

        /// <summary>
        /// POST: Handle multiple PDF file uploads for a specific employee.
        /// Validates each file individually for type and size.
        /// </summary>
        public async Task<IActionResult> OnPostUploadAsync(string employeeId, IList<IFormFile> files)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                TempData["Error"] = "Employee not found.";
                return RedirectToPage("/Admin/Employees");
            }

            var (success, errorMessage) = await _documentService.UploadDocumentsAsync(employeeId, files);

            TempData[success ? "Success" : "Error"] = success
                ? "Documents uploaded successfully."
                : errorMessage;

            return RedirectToPage(new { id = employeeId });
        }

        /// <summary>
        /// POST: Soft delete a document record and remove file from disk.
        /// </summary>
        public async Task<IActionResult> OnPostDeleteDocumentAsync(int id, string employeeId)
        {
            var result = await _documentService.DeleteDocumentAsync(id);

            TempData[result ? "Success" : "Error"] = result
                ? "Document deleted successfully."
                : "Document not found.";

            return RedirectToPage(new { id = employeeId });
        }
    }
}