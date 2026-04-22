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
        /// Saves PDF file to disk and stores metadata in database.
        /// Returns success flag and error message if any.
        /// </summary>
        Task<(bool Success, string ErrorMessage)> UploadDocumentAsync(string employeeId, IFormFile file);

        /// <summary>
        /// Soft deletes document record and removes physical file from disk.
        /// </summary>
        Task<bool> DeleteDocumentAsync(int id);
    }
}