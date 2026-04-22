using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Services.Interfaces
{
    /// <summary>
    /// Defines business logic contract for Employee operations.
    /// Acts as a bridge between Razor Pages and the Repository layer.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>Returns all active employees.</summary>
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();

        /// <summary>Returns a single employee by ID.</summary>
        Task<Employee?> GetEmployeeByIdAsync(string id);

        /// <summary>Creates a new employee with hashed password via Identity.</summary>
        Task<(bool Success, IEnumerable<string> Errors)> CreateEmployeeAsync(AddEmployeeViewModel model);

        /// <summary>Updates an existing employee's details.</summary>
        Task<(bool Success, IEnumerable<string> Errors)> UpdateEmployeeAsync(EditEmployeeViewModel model);

        /// <summary>Soft deletes an employee by ID.</summary>
        Task<bool> SoftDeleteEmployeeAsync(string id);
    }
}