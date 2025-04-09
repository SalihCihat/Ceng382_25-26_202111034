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

        [BindProperty(SupportsGet = true)]
        public string? SearchClassName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public List<ClassInformationTable> FilteredClassList { get; set; } = new();

        public void OnGet()
        {
            // Populate class list with 100 dummy data if it's empty
            if (_classList.Count == 0)
            {
                for (int i = _classList.Count; i < 100; i++)
                {
                    _classList.Add(new ClassInformationModel
                    {
                        Id = _nextId++,
                        ClassName = "Class " + (i % 10),
                        StudentCount = 20 + (i % 5),
                        Description = "Generated class " + i
                    });
                }
            }

            var query = _classList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchClassName))
            {
                query = query.Where(x => x.ClassName.Contains(SearchClassName, StringComparison.OrdinalIgnoreCase));
            }

            int totalItems = query.Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            query = query.Skip((PageNumber - 1) * PageSize).Take(PageSize);

            FilteredClassList = query.Select(x => new ClassInformationTable
            {
                Id = x.Id,
                ClassName = x.ClassName,
                StudentCount = x.StudentCount,
                Description = x.Description
            }).ToList();
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            NewClass.Id = _nextId++;
            _classList.Add(NewClass);

            // After adding, reload the page to show the updated list
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
