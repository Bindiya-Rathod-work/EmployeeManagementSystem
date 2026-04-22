using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories.Interfaces;
using EmployeeManagementSystem.Services.Interfaces;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystem.Services.Implementations
{
    /// <summary>
    /// Implements business logic for Employee operations.
    /// Uses UserManager for identity-related operations and
    /// IEmployeeRepository for data access.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<Employee> _userManager;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            UserManager<Employee> userManager)
        {
            _employeeRepository = employeeRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns all active employees from the repository.
        /// </summary>
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        /// <summary>
        /// Returns a single employee by their unique ID.
        /// </summary>
        public async Task<Employee?> GetEmployeeByIdAsync(string id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a new employee account using ASP.NET Core Identity.
        /// Assigns the "Employee" role upon successful creation.
        /// </summary>
        public async Task<(bool Success, IEnumerable<string> Errors)> CreateEmployeeAsync(AddEmployeeViewModel model)
        {
            // ─── Map ViewModel to Employee entity ────────────────────────
            var employee = new Employee
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Department = model.Department,
                Designation = model.Designation,
                DateOfJoining = DateTime.SpecifyKind(model.DateOfJoining, DateTimeKind.Utc),
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            // ─── Create user with hashed password via Identity ────────────
            var result = await _userManager.CreateAsync(employee, model.Password);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description));

            // ─── Assign Employee role ─────────────────────────────────────
            await _userManager.AddToRoleAsync(employee, "Employee");

            return (true, Enumerable.Empty<string>());
        }

        /// <summary>
        /// Updates an existing employee's profile details.
        /// Email and password changes are handled separately via Identity.
        /// </summary>
        public async Task<(bool Success, IEnumerable<string> Errors)> UpdateEmployeeAsync(EditEmployeeViewModel model)
        {
            var employee = await _employeeRepository.GetByIdAsync(model.Id);

            if (employee == null)
                return (false, new[] { "Employee not found." });

            // ─── Update fields ────────────────────────────────────────────
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Department = model.Department;
            employee.Designation = model.Designation;
            employee.DateOfJoining = DateTime.SpecifyKind(model.DateOfJoining, DateTimeKind.Utc);
            employee.PhoneNumber = model.PhoneNumber;
            employee.Address = model.Address;
            employee.UpdatedAt = DateTime.UtcNow;

            // ─── Update email if changed ──────────────────────────────────
            if (employee.Email != model.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(employee, model.Email);
                if (!setEmailResult.Succeeded)
                    return (false, setEmailResult.Errors.Select(e => e.Description));

                await _userManager.SetUserNameAsync(employee, model.Email);
            }

            await _employeeRepository.UpdateAsync(employee);
            return (true, Enumerable.Empty<string>());
        }

        /// <summary>
        /// Soft deletes an employee. Record is retained in DB for audit trail.
        /// </summary>
        public async Task<bool> SoftDeleteEmployeeAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return false;

            await _employeeRepository.SoftDeleteAsync(id);
            return true;
        }
    }
}