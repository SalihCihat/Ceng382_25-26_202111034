using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using lab5.Models;

namespace lab5.Pages // Namespace'in doğru olduğundan emin olun
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        // OnPost metodu
        public IActionResult OnPost()
        {
            var user = AuthenticateUser(Username, Password);
            if (user != null)
            {
                // Giriş başarılı, oturum açılıyor
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);
                return RedirectToPage("/Index"); // Giriş sonrası yönlendirme
            }

            ErrorMessage = "Invalid username or password.";
            return Page(); // Hata mesajı ile tekrar login sayfasına dönülüyor
        }

        // Kullanıcıyı doğrulayan metot
        private User AuthenticateUser(string username, string password)
        {
            var users = GetUsersFromJson();
            return users?.FirstOrDefault(u => u.Username == username && u.Password == password && u.IsActive);
        }

        // JSON dosyasından kullanıcıları okuyan metot
        private List<User> GetUsersFromJson()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "users.json");

            // JSON dosyasını oku
            if (!System.IO.File.Exists(filePath))
                return null; // Dosya yoksa null döner

            var jsonData = System.IO.File.ReadAllText(filePath); // JSON verisini oku
            return JsonConvert.DeserializeObject<List<User>>(jsonData); // JSON'u User listesine dönüştür
        }
    }
}