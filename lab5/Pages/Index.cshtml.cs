using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace lab5.Pages
{
    public class IndexModel : PageModel
    {
        private static List<ClassInformationModel> _classList = new List<ClassInformationModel>(); // Kaydedilen veriler
        public List<ClassInformationModel> ClassList => _classList;

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        public void OnGet() { }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewClass.Id = _classList.Count + 1; // ID otomatik artırma
            _classList.Add(NewClass);
            return RedirectToPage(); // Sayfa yenilenince veriler kaybolmayacak
        }

        public IActionResult OnPostDelete(int id)
        {
            var classToRemove = _classList.Find(c => c.Id == id);
            if (classToRemove != null)
            {
                _classList.Remove(classToRemove);
            }
            return RedirectToPage();
        }
    }

    public class ClassInformationModel
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public int StudentCount { get; set; }
        public string Description { get; set; }
    }
}
