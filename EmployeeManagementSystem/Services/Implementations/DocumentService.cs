using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories.Interfaces;
using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagementSystem.Services.Implementations
{
    /// <summary>
    /// Implements business logic for Employee Document operations.
    /// Manages both physical file storage and database metadata.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IWebHostEnvironment _environment;

        public DocumentService(
            IDocumentRepository documentRepository,
            IWebHostEnvironment environment)
        {
            _documentRepository = documentRepository;
            _environment = environment;
        }

        /// <summary>
        /// Returns all active documents for a given employee.
        /// </summary>
        public async Task<IEnumerable<EmployeeDocument>> GetDocumentsByEmployeeIdAsync(string employeeId)
        {
            return await _documentRepository.GetByEmployeeIdAsync(employeeId);
        }

        /// <summary>
        /// Returns a single document by its ID.
        /// </summary>
        public async Task<EmployeeDocument?> GetDocumentByIdAsync(int id)
        {
            return await _documentRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Validates and saves multiple PDF files to the Uploads folder on disk.
        /// Processes each file individually and collects errors for failed uploads.
        /// Only PDF files under 10MB are accepted.
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> UploadDocumentsAsync(string employeeId, IList<IFormFile> files)
        {
            // ─── Validate files list ──────────────────────────────────────────
            if (files == null || files.Count == 0)
                return (false, "No files were selected.");

            var errors = new List<string>();

            foreach (var file in files)
            {
                // ─── Validate file type ───────────────────────────────────────
                if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add($"{file.FileName} is not a PDF file.");
                    continue;
                }

                // ─── Validate file size (max 10MB) ────────────────────────────
                if (file.Length > 10 * 1024 * 1024)
                {
                    errors.Add($"{file.FileName} exceeds the 10MB size limit.");
                    continue;
                }

                // ─── Build unique file name to avoid conflicts ────────────────
                var uniqueFileName = $"{employeeId}_{DateTime.UtcNow:yyyyMMddHHmmss}_{file.FileName}";
                var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads");

                // ─── Ensure uploads directory exists ─────────────────────────
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // ─── Save file to disk ────────────────────────────────────────
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ─── Save metadata to database ────────────────────────────────
                var document = new EmployeeDocument
                {
                    EmployeeId = employeeId,
                    FileName = file.FileName,
                    FilePath = filePath,
                    FileType = file.ContentType,
                    FileSizeInBytes = file.Length,
                    UploadedAt = DateTime.UtcNow
                };

                await _documentRepository.AddAsync(document);
            }

            // ─── Return result with any errors encountered ────────────────────
            if (errors.Count > 0)
                return (false, string.Join(" | ", errors));

            return (true, string.Empty);
        }

        /// <summary>
        /// Soft deletes document record in DB and removes physical file from disk.
        /// </summary>
        public async Task<bool> DeleteDocumentAsync(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null) return false;

            // ─── Delete physical file from disk ───────────────────────────
            if (File.Exists(document.FilePath))
                File.Delete(document.FilePath);

            // ─── Soft delete record in database ──────────────────────────
            await _documentRepository.SoftDeleteAsync(id);
            return true;
        }
    }
}