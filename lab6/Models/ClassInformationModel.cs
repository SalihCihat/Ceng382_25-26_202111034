using System.ComponentModel.DataAnnotations;

namespace lab6.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Student count must be at least 1")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
    }
}
