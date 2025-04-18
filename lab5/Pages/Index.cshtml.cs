using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lab5.Models; 
using Microsoft.AspNetCore.Http; 
using System.Collections.Generic;
using System.Linq;
using System;

#nullable enable

namespace lab5.Pages
{
    public class IndexModel : PageModel
    {
        public string? Username { get; set; }
        public string? Role { get; set; }

        public IActionResult OnGet()
        {
            // Oturumdan kullanıcı adı ve rol bilgisini alıyoruz
            Username = HttpContext.Session.GetString("Username");
            Role = HttpContext.Session.GetString("Role");

            // Oturum yoksa giriş sayfasına yönlendiriyoruz
            if (Username == null)
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}
