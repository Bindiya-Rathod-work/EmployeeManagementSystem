using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.ViewModels
{
    /// <summary>
    /// ViewModel for the Employee's own profile view.
    /// Contains only basic non-sensitive information visible to the employee.
    /// </summary>
    public class EmployeeProfileViewModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Department")]
        public string Department { get; set; } = string.Empty;

        [Display(Name = "Designation")]
        public string Designation { get; set; } = string.Empty;

        [Display(Name = "Date of Joining")]
        public DateTime DateOfJoining { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}