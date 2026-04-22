using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    /// <summary>
    /// Represents a PDF document uploaded for a specific employee.
    /// </summary>
    public class EmployeeDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? FileType { get; set; }

        public long FileSizeInBytes { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Soft delete flag for document.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Navigation property back to the Employee.
        /// </summary>
        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
    }
}