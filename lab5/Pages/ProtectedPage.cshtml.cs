using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace lab5.Pages
{
    public class ProtectedPageModel : PageModel
    {
        public string Username { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Check if user is logged in by comparing session and cookies
            var username = HttpContext.Session.GetString("Username");
            var token = HttpContext.Session.GetString("Token");

            if (username == null || token == null || 
                username != Request.Cookies["Username"] || token != Request.Cookies["Token"])
            {
                ErrorMessage = "You must log in first.";
            }
            else
            {
                Username = username;
            }
        }
    }
}
