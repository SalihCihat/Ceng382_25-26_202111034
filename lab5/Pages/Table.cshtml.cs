using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace lab5.Pages
{
    public class TableModel : PageModel
    {
        public string Username { get; set; } = string.Empty;

        public void OnGet()
        {
            // Session’dan kullanıcı bilgisini al
            Username = HttpContext.Session.GetString("username") ?? "Guest";
        }
    }
}
