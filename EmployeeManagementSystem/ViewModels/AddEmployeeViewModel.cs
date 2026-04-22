using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.ViewModels
{
    /// <summary>
    /// ViewModel used for the Add Employee form.
    /// Keeps Entity Model separate from UI concerns.
    /// </summary>
    public class AddEmployeeViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required.")]
        [MaxLength(100)]
        [Display(Name = "Department")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Designation is required.")]
        [MaxLength(100)]
        [Display(Name = "Designation")]
        public string Designation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of joining is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Joining")]
        public DateTime DateOfJoining { get; set; } = DateTime.Today;

        [MaxLength(10)]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [MaxLength(250)]
        [Display(Name = "Address")]
        public string? Address { get; set; }
    }
}