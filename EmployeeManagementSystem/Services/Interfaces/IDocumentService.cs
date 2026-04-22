using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagementSystem.Services.Interfaces
{
    /// <summary>
    /// Defines business logic contract for Employee Document operations.
    /// Handles both file system and database operations for PDFs.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>Returns all active documents for a specific employee.</summary>
        Task<IEnumerable<EmployeeDocument>> GetDocumentsByEmployeeIdAsync(string employeeId);

        /// <summary>Returns a single document by ID.</summary>
        Task<EmployeeDocument?> GetDocumentByIdAsync(int id);

        /// <summary>
        /// Saves multiple PDF files to disk and stores metadata in database.
        /// Returns list of errors for any failed uploads.
        /// </summary>
        Task<(bool Success, string ErrorMessage)> UploadDocumentsAsync(string employeeId, IList<IFormFile> files);

        /// <summary>
        /// Soft deletes document record and removes physical file from disk.
        /// </summary>
        Task<bool> DeleteDocumentAsync(int id);
    }
}