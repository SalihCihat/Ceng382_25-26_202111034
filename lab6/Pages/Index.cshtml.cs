using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lab6.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lab6.Pages
{
    public class IndexModel : PageModel
    {
        // In-memory "veritabanı" – uygulama çalıştığı sürece kayıtlar saklanır.
        public static List<ClassInformationModel> ClassList { get; set; } = new List<ClassInformationModel>();

        // Filtrelenmiş ve sayfalı veriler, tabloda görüntülenecek.
        public List<ClassInformationTable> DisplayTable { get; set; } = new List<ClassInformationTable>();

        // Filtreleme için arama kelimesi (GET query parametresi)
        [BindProperty(SupportsGet = true)]
        public string SearchKeyword { get; set; } = "";

        // Sayfalama için sayfa numarası (PageModel.Page ile çakışmaması için "CurrentPage")
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        // Add / Edit işlemleri için form verisi
        [BindProperty]
        public ClassInformationModel ClassInfo { get; set; } = new ClassInformationModel();

        // Sahte verilerin üretilip üretilmediğini kontrol etmek için statik bayrak
        private static bool dataGenerated = false;

        public void OnGet()
        {
            if (!dataGenerated)
            {
                GenerateSampleData();
                dataGenerated = true;
            }

            // Filtreleme: SearchKeyword boşsa tüm kayıtlar, değilse ClassName'de arama yapılır.
            var filtered = string.IsNullOrWhiteSpace(SearchKeyword)
                ? ClassList
                : ClassList.Where(c => c.ClassName.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)).ToList();

            // Sayfalama: Toplam sayfa sayısı hesaplanır.
            TotalPages = (int)Math.Ceiling(filtered.Count / (double)PageSize);

            // İlgili sayfadaki veriler alınır.
            var paginated = filtered
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // DisplayTable, tablo modeline dönüştürülür.
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

            // Otomatik artan ID ataması
            ClassInfo.Id = ClassList.Count > 0 ? ClassList.Max(c => c.Id) + 1 : 1;
            ClassList.Add(ClassInfo);

            // Yeni kayıt eklendikten sonra filtreyi temizleyip ilk sayfaya yönlendir.
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
                // Düzenleme için formu doldur.
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
