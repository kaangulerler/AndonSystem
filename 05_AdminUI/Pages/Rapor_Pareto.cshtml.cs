using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "RaporPolicy")]
    public class Rapor_ParetoModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
