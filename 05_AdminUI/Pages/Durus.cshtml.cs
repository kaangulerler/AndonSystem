using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "DurusTipPolicy")]

    public class DurusModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
