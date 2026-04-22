using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories.Implementations
{
    /// <summary>
    /// Handles all database operations related to EmployeeDocument entity.
    /// </summary>
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all active documents for a given employee.
        /// Soft-deleted documents are excluded via global query filter.
        /// </summary>
        public async Task<IEnumerable<EmployeeDocument>> GetByEmployeeIdAsync(string employeeId)
        {
            return await _context.EmployeeDocuments
                .Where(d => d.EmployeeId == employeeId)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a single document by ID.
        /// Returns null if not found or soft-deleted.
        /// </summary>
        public async Task<EmployeeDocument?> GetByIdAsync(int id)
        {
            return await _context.EmployeeDocuments
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        /// <summary>
        /// Saves a new document metadata record to the database.
        /// Actual file is stored on disk by the Service layer.
        /// </summary>
        public async Task AddAsync(EmployeeDocument document)
        {
            await _context.EmployeeDocuments.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Soft deletes a document record by setting IsDeleted = true.
        /// Physical file deletion is handled by the Service layer.
        /// </summary>
        public async Task SoftDeleteAsync(int id)
        {
            var document = await _context.EmployeeDocuments
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (document != null)
            {
                document.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}