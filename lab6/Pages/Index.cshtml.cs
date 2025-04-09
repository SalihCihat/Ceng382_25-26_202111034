using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lab6.Models;
using lab6.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab6.Pages
{
    public class IndexModel : PageModel
    {
        public static List<ClassInformationModel> ClassList { get; set; } = new List<ClassInformationModel>();

        public List<ClassInformationTable> DisplayTable { get; set; } = new List<ClassInformationTable>();

        [BindProperty(SupportsGet = true)]
        public string SearchKeyword { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        [BindProperty]
        public ClassInformationModel ClassInfo { get; set; } = new ClassInformationModel();

        [BindProperty]
        public List<string> SelectedColumns { get; set; } = new List<string>();

        private static bool dataGenerated = false;

        public void OnGet()
        {
            if (!dataGenerated)
            {
                GenerateSampleData();
                dataGenerated = true;
            }

            var filtered = string.IsNullOrWhiteSpace(SearchKeyword)
                ? ClassList
                : ClassList.Where(c => c.ClassName.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)).ToList();

            TotalPages = (int)Math.Ceiling(filtered.Count / (double)PageSize);

            var paginated = filtered
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            DisplayTable = paginated.Select(c => new ClassInformationTable
            {
                Id = c.Id,
                ClassName = c.ClassName,
                StudentCount = c.StudentCount,
                Description = c.Description
            }).ToList();
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            ClassInfo.Id = ClassList.Count > 0 ? ClassList.Max(c => c.Id) + 1 : 1;
            ClassList.Add(ClassInfo);

            return RedirectToPage("./Index", new { CurrentPage = 1, SearchKeyword = "" });
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
                ClassList.Remove(item);

            return RedirectToPage("./Index", new { CurrentPage, SearchKeyword });
        }

        public IActionResult OnPostEdit(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                ClassInfo = new ClassInformationModel
                {
                    Id = item.Id,
                    ClassName = item.ClassName,
                    StudentCount = item.StudentCount,
                    Description = item.Description
                };
            }
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
            return RedirectToPage("./Index", new { CurrentPage, SearchKeyword });
        }

        public IActionResult OnPostExportUnfiltered()
        {
            string json = Utils.Instance.ExportToJson(ClassList, SelectedColumns);
            byte[] fileBytes = Encoding.UTF8.GetBytes(json);
            return File(fileBytes, "application/json", "ExportUnfiltered.json");
        }

        public IActionResult OnPostExportFiltered()
        {
            var filtered = string.IsNullOrWhiteSpace(SearchKeyword)
                ? ClassList
                : ClassList.Where(c => c.ClassName.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)).ToList();
            string json = Utils.Instance.ExportToJson(filtered, SelectedColumns);
            byte[] fileBytes = Encoding.UTF8.GetBytes(json);
            return File(fileBytes, "application/json", "ExportFiltered.json");
        }

        private void GenerateSampleData()
        {
            if (ClassList.Count == 0)
            {
                for (int i = 1; i <= 100; i++)
                {
                    ClassList.Add(new ClassInformationModel
                    {
                        Id = i,
                        ClassName = $"Class {i}",
                        StudentCount = (i % 30) + 1,
                        Description = $"Description for Class {i}"
                    });
                }
            }
        }
    }
}
