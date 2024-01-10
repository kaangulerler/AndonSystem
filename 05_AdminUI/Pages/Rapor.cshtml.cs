using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "RaporPolicy")]

    public class RaporModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
