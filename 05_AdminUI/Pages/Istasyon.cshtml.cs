using _01_DbModel.AppModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "IstasyonPolicy")]

    public class IstasyonModel : PageModel
    {
        IEnumerable<T3_Hat> List_Hat { get; set; } = new List<T3_Hat>();

        public void OnGet()
        {
            List_Hat =  Enum.GetValues(typeof(T3_Hat)).Cast<T3_Hat>();
        }
    }
}
