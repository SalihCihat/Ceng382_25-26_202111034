using System.ComponentModel.DataAnnotations;

namespace lab5.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name is required")]
        public string? ClassName { get; set; } // Nullable

        [Required(ErrorMessage = "Student Count is required")]
        [Range(1, 500, ErrorMessage = "Student count must be between 1 and 500")]
        public int StudentCount { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; } // Nullable
    }
}
