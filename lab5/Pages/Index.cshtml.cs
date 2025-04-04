using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lab5.Models;

namespace lab5.Pages
{
    public class IndexModel : PageModel
    {
        // Statik liste, uygulama çalıştığı sürece verileri saklar
        public static List<ClassInformationModel> ClassList { get; set; } = new List<ClassInformationModel>();

        [BindProperty]
        public ClassInformationModel ClassInfo { get; set; } = new();

        public void OnGet() { }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            // Otomatik artan ID
            ClassInfo.Id = ClassList.Count > 0 ? ClassList.Max(c => c.Id) + 1 : 1;
            ClassList.Add(ClassInfo);
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
                ClassList.Remove(item);

            return RedirectToPage();
        }

        public IActionResult OnPostEdit(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
                ClassInfo = new ClassInformationModel
                {
                    Id = item.Id,
                    ClassName = item.ClassName,
                    StudentCount = item.StudentCount,
                    Description = item.Description
                };
            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            var item = ClassList.FirstOrDefault(c => c.Id == ClassInfo.Id);
            if (item != null)
            {
                item.ClassName = ClassInfo.ClassName;
                item.StudentCount = ClassInfo.StudentCount;
                item.Description = ClassInfo.Description;
            }
            return RedirectToPage();
        }
    }
}
