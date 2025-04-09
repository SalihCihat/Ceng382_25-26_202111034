using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lab5.Models;

namespace lab5.Pages
{
    public class IndexModel : PageModel
    {
        private static List<ClassInformationModel> _classList = new();
        private static int _nextId = 1;

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        [BindProperty]
        public bool IsEditing { get; set; }

        [BindProperty]
        public int EditId { get; set; }

        public List<ClassInformationModel> ClassList => _classList;

        public void OnGet()
        {
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            NewClass.Id = _nextId++;
            _classList.Add(NewClass);
            return RedirectToPage();
        }

        public IActionResult OnPostEdit(int id)
        {
            var item = _classList.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return RedirectToPage();

            NewClass = new ClassInformationModel
            {
                Id = item.Id,
                ClassName = item.ClassName,
                StudentCount = item.StudentCount,
                Description = item.Description
            };

            IsEditing = true;
            EditId = id;

            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var item = _classList.FirstOrDefault(x => x.Id == EditId);
            if (item == null)
                return RedirectToPage();

            item.ClassName = NewClass.ClassName;
            item.StudentCount = NewClass.StudentCount;
            item.Description = NewClass.Description;

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = _classList.FirstOrDefault(x => x.Id == id);
            if (item != null)
                _classList.Remove(item);

            return RedirectToPage();
        }
    }
}
