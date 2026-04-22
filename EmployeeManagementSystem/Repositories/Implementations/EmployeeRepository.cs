using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories.Implementations
{
    /// <summary>
    /// Handles all database operations related to Employee entity.
    /// Uses ApplicationDbContext via dependency injection.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Employee> _userManager;

        public EmployeeRepository(
            ApplicationDbContext context,
            UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns all active employees excluding users assigned to Admin role.
        /// Soft-deleted employees are automatically excluded via global query filter.
        /// </summary>
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            // ─── Get all Admin user IDs from Identity role table ──────────
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            var adminIds = adminUsers.Select(a => a.Id).ToHashSet();

            // ─── Exclude admins from employee list ────────────────────────
            return await _context.Users
                .Include(e => e.Documents.Where(d => !d.IsDeleted))
                .Where(e => !adminIds.Contains(e.Id))
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a single employee with their documents by ID.
        /// Returns null if not found or soft-deleted.
        /// </summary>
        public async Task<Employee?> GetByIdAsync(string id)
        {
            return await _context.Users
                .Include(e => e.Documents.Where(d => !d.IsDeleted))
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Persists a new employee record to the database.
        /// Note: Password hashing is handled by UserManager, not here.
        /// </summary>
        public async Task AddAsync(Employee employee)
        {
            await _context.Users.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing employee record in the database.
        /// Sets UpdatedAt timestamp before saving.
        /// </summary>
        public async Task UpdateAsync(Employee employee)
        {
            employee.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(employee);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Performs a soft delete by setting IsDeleted = true.
        /// Employee record remains in the database for audit purposes.
        /// </summary>
        public async Task SoftDeleteAsync(string id)
        {
            var employee = await _context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee != null)
            {
                employee.IsDeleted = true;
                employee.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Returns all soft deleted employees.
        /// IgnoreQueryFilters is used to bypass the global IsDeleted filter.
        /// </summary>
        public async Task<IEnumerable<Employee>> GetInactiveAsync()
        {
            // ─── Get all Admin IDs to exclude from list ───────────────────────
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            var adminIds = adminUsers.Select(a => a.Id).ToHashSet();

            return await _context.Users
                .IgnoreQueryFilters()
                .Include(e => e.Documents.Where(d => !d.IsDeleted))
                .Where(e => e.IsDeleted && !adminIds.Contains(e.Id))
                .OrderByDescending(e => e.UpdatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Reactivates a soft deleted employee by setting IsDeleted = false.
        /// </summary>
        public async Task ReactivateAsync(string id)
        {
            var employee = await _context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee != null)
            {
                employee.IsDeleted = false;
                employee.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}