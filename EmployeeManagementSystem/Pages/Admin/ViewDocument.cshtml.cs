using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Admin
{
    /// <summary>
    /// Serves a PDF document file directly to the browser for inline viewing.
    /// Streams the file from disk using its stored file path.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ViewDocumentModel : PageModel
    {
        private readonly IDocumentService _documentService;

        public ViewDocumentModel(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        /// <summary>
        /// GET: Stream the PDF file to the browser for inline viewing.
        /// Returns 404 if document is not found or file does not exist on disk.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            // ─── Serve PDF inline so browser renders it inside the iframe ────
            var fileBytes = await System.IO.File.ReadAllBytesAsync(document.FilePath);
            Response.Headers.Append("Content-Disposition", "inline; filename=" + document.FileName);
            return File(fileBytes, "application/pdf");


            //if (document == null || !System.IO.File.Exists(document.FilePath))
            //    return NotFound("Document not found.");

            //// ─── Stream file directly to browser ─────────────────────────
            //var fileBytes = await System.IO.File.ReadAllBytesAsync(document.FilePath);
            //return File(fileBytes, "application/pdf", document.FileName);
        }
    }
}