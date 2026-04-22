using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data
{
    /// <summary>
    /// Main database context for the Employee Management System.
    /// Extends IdentityDbContext to include ASP.NET Core Identity tables.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<Employee>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet for employee documents.
        /// </summary>
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ─── Employee global query filter ───────────────────────────
            // Automatically excludes soft-deleted employees from all queries
            builder.Entity<Employee>()
                .HasQueryFilter(e => !e.IsDeleted);

            // ─── EmployeeDocument global query filter ────────────────────
            // Automatically excludes soft-deleted documents from all queries
            builder.Entity<EmployeeDocument>()
                .HasQueryFilter(d => !d.IsDeleted);

            // ─── Employee → Documents relationship ──────────────────────
            builder.Entity<Employee>()
                .HasMany(e => e.Documents)
                .WithOne(d => d.Employee)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ─── Rename Identity tables to cleaner names ─────────────────
            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
    }
}