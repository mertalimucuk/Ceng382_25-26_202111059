using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Week5Lab.Models;

namespace Week5Lab.Pages
{
    public class IndexModel : PageModel
    {
        // Statik liste (veriler burada tutulacak)
        public static List<ClassInformationModel> ClassList { get; set; } = new();

        // Formdan gelen veri (model binding)
        [BindProperty]
        public ClassInformationModel NewClass { get; set; }

        public void OnGet()
        {
            // Sayfa yüklendiğinde yapılacak işlemler
        }

        public IActionResult OnPostAdd()
        {
            // Otomatik ID
            NewClass.Id = ClassList.Count > 0 ? ClassList.Max(c => c.Id) + 1 : 1;

            // Listeye ekle
            ClassList.Add(NewClass);

            // Sayfayı yenile
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                ClassList.Remove(item);
            }
            return RedirectToPage();
        }
    }
}

