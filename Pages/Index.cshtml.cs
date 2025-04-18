using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Week5Lab.Models;

namespace Week5Lab.Pages
{
    public class IndexModel : PageModel
    {
        public static List<ClassInformationModel> ClassList { get; set; } = new();
        public List<ClassInformationTable> FilteredList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        public bool IsEditing { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedId { get; set; }

        [BindProperty]
        public List<string> SelectedColumns { get; set; } = new();

        public void OnGet()
        {
            SeedData(); //  sadece ilk girişte çağrılıyor

            if (SelectedId.HasValue)
            {
                var item = ClassList.FirstOrDefault(c => c.Id == SelectedId.Value);
                if (item != null)
                {
                    NewClass = new ClassInformationModel
                    {
                        Id = item.Id,
                        ClassName = item.ClassName,
                        StudentCount = item.StudentCount,
                        Description = item.Description
                    };
                    IsEditing = true;
                }
            }

            var query = ClassList.AsQueryable();
            //AI PROMPT:Sitemin serch kısmında ARAMA KELİMESİNE UYGUN OLARAK FİLTRELEME NASIL YAPABİİRİM ?
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(c => c.ClassName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            TotalPages = (int)Math.Ceiling(query.Count() / (double)PageSize);
            //AI Prompt : Kendi sayfamda bulunduğum sayfanın verilerini nasıl alabilirim ?
            var paged = query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            
            FilteredList = paged.Select(c => new ClassInformationTable
            {
                ClassName = c.ClassName,
                StudentCount = c.StudentCount,
                Description = c.Description
            }).ToList();
        }

        public IActionResult OnPostAdd()
        {
            NewClass.Id = ClassList.Count > 0 ? ClassList.Max(c => c.Id) + 1 : 1;
            ClassList.Add(NewClass);
            return RedirectToPage();
        }

        public IActionResult OnPostUpdate()
        {
            var existing = ClassList.FirstOrDefault(c => c.Id == NewClass.Id);
            if (existing != null)
            {
                existing.ClassName = NewClass.ClassName;
                existing.StudentCount = NewClass.StudentCount;
                existing.Description = NewClass.Description;
            }

            // AI PROMPT: AramA FİLTRESİNİ koruyarak sayfayı yeniden nasıl yükleyebilirim.
            return RedirectToPage(new { SearchTerm });
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                ClassList.Remove(item);
            }

            // ID'leri yeniden sırala
            for (int i = 0; i < ClassList.Count; i++)
            {
                ClassList[i].Id = i + 1;
            }

            //AI PROMPT: SAYFA SAYISI GÜNCELLEMESİ SONRASI SAYFA SINIRIMI NASIL KORUYABİLİRİM ?
            var filteredCount = ClassList
                .Where(c => string.IsNullOrWhiteSpace(SearchTerm) || c.ClassName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                .Count();

            int totalPages = (int)Math.Ceiling(filteredCount / (double)PageSize);

            if (PageNumber > totalPages)
            {
                PageNumber = totalPages;
            }

            return RedirectToPage(new { PageNumber, SearchTerm });
        }

        public IActionResult OnPostEdit(int id)
        {
            return RedirectToPage(new { SelectedId = id });
        }

        // Sadece ilk açılışta çağrılır, bir daha asla
        public void SeedData()
        {
            if (ClassList.Count > 0) return; // zaten doluysa basma

            var rnd = new Random();
            for (int i = 1; i <= 100; i++)
            {
                ClassList.Add(new ClassInformationModel
                {
                    Id = i,
                    ClassName = $"Class {i}",
                    StudentCount = rnd.Next(10, 100),
                    Description = $"Description for Class {i}"
                });
            }
        }

        public IActionResult OnPostExport()
        {
            var query = ClassList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(c => c.ClassName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var paged = query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var json = Utils.Instance.ExportToJson(paged);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "paged_export.json");
        }

         //AI PROMPT: GEREKLİ SAYFAMDA SEÇİLEN KOLONLARI JSON OLARAK NASIL EXPORT EDEBİLİRİM ?   
        public IActionResult OnPostExportFiltered()
        {
            var query = ClassList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(c => c.ClassName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var paged = query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(c => new ClassInformationTable
                {
                    ClassName = c.ClassName,
                    StudentCount = c.StudentCount,
                    Description = c.Description
                })
                .ToList();

            var json = Utils.Instance.ExportToJson(paged, SelectedColumns);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "filtered_export.json");
        }
    }
}
