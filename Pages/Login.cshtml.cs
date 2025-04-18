using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // 👉 Session için gerekli

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
            // 🔐 Kullanıcı adı ve şifre kontrolü
            if (Username == "admin" && Password == "123")
            {
                // ✅ Session başlat
                HttpContext.Session.SetString("Username", Username);
                HttpContext.Session.SetString("Section", "4");

                // 🔁 Index'e yönlendir
                return RedirectToPage("/Index");
            }

            // ❌ Hatalı giriş
            ErrorMessage = "Invalid username or password";
            return Page();
        }
    }
}
