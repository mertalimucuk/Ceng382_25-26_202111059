using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Week5Lab.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

//dotnet run --project Week5Lab.csproj --urls=http://localhost:5000  hocam terminalde bu komutu girince kendi bilgisayarımda çalışıyor direkt olarak dotnet runda sorun yaşadım. bu komutu girince ilgili proje dosyam çalıştı dotnet runda sorun yaşamanız halinde bu komutu girip localhostu açabilirseniz sevinirim hocam
namespace Week5Lab.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Ceng382DbContext _db = new();

        public List<ClassInformation> FilteredList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        [BindProperty]
        public List<string> SelectedColumns { get; set; } = new();

        [BindProperty]
        public ClassInformation EditableClass { get; set; } = new();

        public string? Username { get; set; }
        public string? SessionId { get; set; }
        public string? CookieUsername { get; set; }

        // AI Prompt: "Check login session and load paginated, filtered class list from database"
        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            SessionId = HttpContext.Session.Id;
            CookieUsername = Request.Cookies["Username"];

            if (string.IsNullOrEmpty(Username))
                return RedirectToPage("/Login");

            var query = _db.ClassInformation.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(c => c.ClassName.Contains(SearchTerm));
            }

            TotalPages = (int)Math.Ceiling(query.Count() / (double)PageSize);

            FilteredList = query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return Page();
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid) return Page();

            _db.ClassInformation.Add(EditableClass);
            _db.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var entity = _db.ClassInformation.FirstOrDefault(c => c.Id == id);
            if (entity != null)
            {
                _db.ClassInformation.Remove(entity);
                _db.SaveChanges();
            }

            return RedirectToPage();
        }
        
        // AI Prompt: "Load selected class data into EditableClass for editing"
        public IActionResult OnPostEdit(int id)
        {
            var item = _db.ClassInformation.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                EditableClass = new ClassInformation
                {
                    Id = item.Id,
                    ClassName = item.ClassName,
                    StudentCount = item.StudentCount,
                    Description = item.Description
                };
            }
            return Page();
        }

        // AI Prompt: "Update class record in the database with edited data"
        public IActionResult OnPostUpdate()
        {
            var existing = _db.ClassInformation.FirstOrDefault(c => c.Id == EditableClass.Id);
            if (existing != null)
            {
                existing.ClassName = EditableClass.ClassName;
                existing.StudentCount = EditableClass.StudentCount;
                existing.Description = EditableClass.Description;

                _db.SaveChanges();
            }
            return RedirectToPage();
        }

        // AI Prompt: "Export the currently filtered and paginated class list as JSON"
        public IActionResult OnPostExportFiltered()
        {
            var query = _db.ClassInformation.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(c => c.ClassName.Contains(SearchTerm));
            }

            var listForExport = query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(c => new ClassInformation
                {
                    ClassName = c.ClassName,
                    StudentCount = c.StudentCount,
                    Description = c.Description
                })
                .ToList();

            var json = Utils.Instance.ExportToJson(listForExport, SelectedColumns);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "filtered_export.json");
        }

        public async Task<IActionResult> OnPostImport()
{
    Console.WriteLine("Import tetiklendi mi?");

    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "filtered_export.json");

    if (!System.IO.File.Exists(jsonPath))
    {
        Console.WriteLine("Dosya bulunamadı.");
        return NotFound("JSON dosyası bulunamadı.");
    }

    var jsonData = await System.IO.File.ReadAllTextAsync(jsonPath);
    Console.WriteLine("Dosya içeriği: " + jsonData.Substring(0, Math.Min(200, jsonData.Length)));

    var importedList = JsonSerializer.Deserialize<List<ClassInformation>>(jsonData);

    if (importedList == null || !importedList.Any())
    {
        Console.WriteLine("Deserialize başarısız veya boş liste.");
        return RedirectToPage();
    }

    int addedCount = 0;

    foreach (var item in importedList)
    {
        var exists = _db.ClassInformation.Any(c =>
            c.ClassName == item.ClassName &&
            c.StudentCount == item.StudentCount &&
            c.Description == item.Description);

        if (!exists)
        {
            _db.ClassInformation.Add(item);
            addedCount++;
        }
    }

    await _db.SaveChangesAsync();
    Console.WriteLine($"✅ Import tamamlandı. {addedCount} yeni kayıt eklendi.");

    return RedirectToPage();
}


        public IActionResult OnPostLogout()
        {
            
            HttpContext.Session.Clear();
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("SessionId");
            return RedirectToPage("/Login");
        }
    }
}
