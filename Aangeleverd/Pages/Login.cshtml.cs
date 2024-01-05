using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Aangeleverd.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty, Required(ErrorMessage ="Voer een gebruikersnaam in"), Display(Name = "Gebruikersnaam:")]
        public string gebruikersnaam { get; set; }
        [BindProperty, Required(ErrorMessage ="Voer een wachtwoord in"), Display(Name = "Wachtwoord:")]
        public string wachtwoord { get; set; }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (gebruikersnaam == "admin" && wachtwoord == "#1Geheim")
                {
                    HttpContext.Session.SetString("gebruiker", "admin");
                    return RedirectToPage("Beheer");
                }
                else
                {
                    ModelState.AddModelError("wachtwoord", "Gebruikersnaam of wachtwoord is onjuist");
                    return Page();
                }
            }
            
            return Page();
        }

        public void OnGet()
        {
        }
    }
}
