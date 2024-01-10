using _05_AdminUI.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "KullanıcıPolicy")]

    public class KullanıcıModel : PageModel
    {
        private readonly UserManager<T3IdentityUser> _userManager;

        public KullanıcıModel(UserManager<T3IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string KullanıcıId { get; set; } = string.Empty;

        public class Kullanıcı
        {
            public string Username { get; set; } = string.Empty;
            public List<Claim> ClaimList { get; set; } = new List<Claim>();
        }

        public List<Kullanıcı> Kullanıcı_Listesi = new List<Kullanıcı>();

        [BindProperty]
        public List<string> YetkiList { get; set; } = new List<string>();

        public class InputModel
        {
            [Required]
            [Display(Name = "Kullanıcı Adı")]
            public string Username { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "Şifre uzunluğu en az {0} karakter olmalıdır.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Şifre")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Şifre Tekrar")]
            [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
            public string ConfirmPassword { get; set; } = string.Empty;

        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string ReturnUrl { get; set; } = string.Empty;


        public void OnGet()
        {

            foreach (var Kullanıcı in _userManager.Users.ToList())
            {
                Kullanıcı_Listesi.Add(new Kullanıcı
                {
                    Username = Kullanıcı.UserName,
                    ClaimList = _userManager.GetClaimsAsync(Kullanıcı).Result.ToList()
                });
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(new T3IdentityUser
                {
                    UserName = Input.Username,
                    Email = Input.Username,
                },
                Input.Password);

                if (result.Succeeded)
                {
                    var kullanıcı = await _userManager.FindByNameAsync(Input.Username);
                    if (kullanıcı != null)
                    {
                        foreach (var yetki in YetkiList)
                            await _userManager.AddClaimAsync(kullanıcı, new Claim ("Sayfa", yetki));
                    }
                }

                return RedirectToPage();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var silinecek = await _userManager.FindByNameAsync(id);

            if (silinecek != null)
                await _userManager.DeleteAsync(silinecek);


            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostKaydetAsync(string id)
        {
            var kullanıcı = await _userManager.FindByNameAsync(id);

            if(kullanıcı != null)
            {
                var klaimler = await _userManager.GetClaimsAsync(kullanıcı);

                foreach(var klaim in klaimler)
                {
                    await _userManager.RemoveClaimAsync(kullanıcı, klaim);
                }

                foreach(var yetki in YetkiList)
                {
                    await _userManager.AddClaimAsync(kullanıcı, new Claim("Sayfa", yetki));
                }
            }
             
            return RedirectToPage();
        }
    }
}
