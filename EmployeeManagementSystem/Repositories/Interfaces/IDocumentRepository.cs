using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    /// <summary>
    /// Defines data access contract for EmployeeDocument operations.
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>Returns all active documents for a specific employee.</summary>
        Task<IEnumerable<EmployeeDocument>> GetByEmployeeIdAsync(string employeeId);

        /// <summary>Returns a single document by its ID.</summary>
        Task<EmployeeDocument?> GetByIdAsync(int id);

        /// <summary>Saves a new document record to the database.</summary>
        Task AddAsync(EmployeeDocument document);

        /// <summary>Soft deletes a document by setting IsDeleted = true.</summary>
        Task SoftDeleteAsync(int id);
    }
}