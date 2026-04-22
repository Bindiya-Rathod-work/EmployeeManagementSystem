using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    /// <summary>
    /// Defines data access contract for Employee entity operations.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>Returns all active (non-deleted) employees.</summary>
        Task<IEnumerable<Employee>> GetAllAsync();

        /// <summary>Returns a single employee by their ID.</summary>
        Task<Employee?> GetByIdAsync(string id);

        /// <summary>Adds a new employee to the database.</summary>
        Task AddAsync(Employee employee);

        /// <summary>Updates an existing employee's details.</summary>
        Task UpdateAsync(Employee employee);

        /// <summary>Soft deletes an employee by setting IsDeleted = true.</summary>
        Task SoftDeleteAsync(string id);
    }
}