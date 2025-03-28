using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using lab5.Models;

namespace lab5.Pages
{
    public class IndexModel : PageModel
    {
        // Static list acting as an in-memory database
        public static List<ClassInformationModel> ClassList { get; set; } = new List<ClassInformationModel>();
        private static int nextId = 1; // Auto-incrementing ID

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

        [BindProperty]
        public int? EditId { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // If validation fails, return to the same page
            }

            NewClass.Id = nextId++; // Assign auto-incremented ID
            ClassList.Add(NewClass);
            return RedirectToPage(); // Refresh the page
        }

        public IActionResult OnPostEdit(int id)
        {
            var classToEdit = ClassList.FirstOrDefault(c => c.Id == id);
            if (classToEdit != null)
            {
                NewClass = new ClassInformationModel
                {
                    Id = classToEdit.Id,
                    ClassName = classToEdit.ClassName,
                    StudentCount = classToEdit.StudentCount,
                    Description = classToEdit.Description
                };
                EditId = id;
            }
            return Page();
        }

        public IActionResult OnPostUpdate(int id)
        {
            var classToUpdate = ClassList.FirstOrDefault(c => c.Id == id);
            if (classToUpdate != null)
            {
                classToUpdate.ClassName = NewClass.ClassName;
                classToUpdate.StudentCount = NewClass.StudentCount;
                classToUpdate.Description = NewClass.Description;
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var classToRemove = ClassList.FirstOrDefault(c => c.Id == id);
            if (classToRemove != null)
            {
                ClassList.Remove(classToRemove);
            }
            return RedirectToPage();
        }
    }
}
