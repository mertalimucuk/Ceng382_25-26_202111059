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

        // Edit modu
        public bool IsEditing { get; set; }

        // Düzenlenen öğenin ID’si
        [BindProperty(SupportsGet = true)]
        public int? SelectedId { get; set; }


        
        public void OnGet()
        {
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
        }
         //AI PROMPT: ADD A NEW CLASS WİTH PROVİDED DETAİLS
        public IActionResult OnPostAdd()
        {
            NewClass.Id = ClassList.Count > 0 ? ClassList.Max(c => c.Id) + 1 : 1;
            ClassList.Add(NewClass);
            return RedirectToPage();
        }
        //AI PROMPT : UPDATE THE CLASS WİTH ID USİNG THE NEW FORM VALUES
        public IActionResult OnPostUpdate()
        {
            var existing = ClassList.FirstOrDefault(c => c.Id == NewClass.Id);
            if (existing != null)
            {
                existing.ClassName = NewClass.ClassName;
                existing.StudentCount = NewClass.StudentCount;
                existing.Description = NewClass.Description;
            }
            return RedirectToPage();
        }

        // AI Prompt: "Delete the class with ID  and reorder the remaining class IDs."
        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                ClassList.Remove(item);
            }

            // ID’leri sıfırla (1, 2, 3 şeklinde yeniden sırala)
            for (int i = 0; i < ClassList.Count; i++)
            {
                ClassList[i].Id = i + 1;
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEdit(int id)
        {
            return RedirectToPage(new { SelectedId = id });
        }
    }
}
