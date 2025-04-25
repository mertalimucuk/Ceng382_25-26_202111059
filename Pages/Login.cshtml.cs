using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // Session için gerekli
using System.Security.Cryptography; // Token üretmek için
using System.Text.Json; //  JSON dosyasını parse etmek için
using Week5Lab.Models; // User modeline erişim

namespace Week5Lab.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            //  JSON'dan kullanıcıları oku
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/users.json");
            if (!System.IO.File.Exists(path))
            {
                ErrorMessage = "User data file not found.";
                return Page();
            }

            var json = System.IO.File.ReadAllText(path);
            var users = JsonSerializer.Deserialize<List<User>>(json);

            // 👤 Kullanıcıyı bul
            var user = users?.FirstOrDefault(u => u.Username == Username && u.Password == Password && u.IsActive);
            if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            //  Token üret
            var token = Guid.NewGuid().ToString();
            var sessionId = HttpContext.Session.Id;

            // Session'a kaydet
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Token", token);
            HttpContext.Session.SetString("SessionId", sessionId);

            // Cookie ayarları
            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            // Cookie'lere yaz
            Response.Cookies.Append("Username", user.Username, options);
            Response.Cookies.Append("Token", token, options);
            Response.Cookies.Append("SessionId", sessionId, options);

            // 🔁 Index'e yönlendir
            return RedirectToPage("/Index");
        }
    }
}
