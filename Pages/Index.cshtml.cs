using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Week5Lab.Models;
using System.Text.Json;
using System.Security.Claims;

namespace Week5Lab.Pages
{
    [Authorize] // ⛔ Giriş yapmayan kullanıcıyı otomatik login sayfasına yollar
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

        public IActionResult OnGet()
        {
            Username = User.Identity?.Name; // Identity üzerinden kullanıcı adı (email) al
            var query = _db.ClassInformation.AsQueryable();

            query = query.Where(c => c.IsActive); // Sadece aktif kayıtlar

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

            EditableClass.IsActive = true;

            _db.ClassInformation.Add(EditableClass);
            _db.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var entity = _db.ClassInformation.FirstOrDefault(c => c.Id == id);
            if (entity != null)
            {
                entity.IsActive = false;
                _db.SaveChanges();
            }

            return RedirectToPage();
        }

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
                    Description = item.Description,
                    IsActive = item.IsActive
                };
            }
            return Page();
        }

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

        public IActionResult OnPostExportFiltered()
        {
            var query = _db.ClassInformation.AsQueryable();

            query = query.Where(c => c.IsActive);

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
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "filtered_export.json");

            if (!System.IO.File.Exists(jsonPath))
                return NotFound("JSON dosyası bulunamadı.");

            var jsonData = await System.IO.File.ReadAllTextAsync(jsonPath);

            var importedList = JsonSerializer.Deserialize<List<ClassInformation>>(jsonData);

            if (importedList == null || !importedList.Any())
                return RedirectToPage();

            foreach (var item in importedList)
            {
                var exists = _db.ClassInformation.Any(c =>
                    c.ClassName == item.ClassName &&
                    c.StudentCount == item.StudentCount &&
                    c.Description == item.Description);

                if (!exists)
                {
                    item.IsActive = true;
                    _db.ClassInformation.Add(item);
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
