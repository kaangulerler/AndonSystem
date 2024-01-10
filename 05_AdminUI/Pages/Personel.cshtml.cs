using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "PersonelPolicy")]

    public class PersonelModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
