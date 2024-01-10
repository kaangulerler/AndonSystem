using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "VardiyaPolicy")]

    public class VardiyaModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
