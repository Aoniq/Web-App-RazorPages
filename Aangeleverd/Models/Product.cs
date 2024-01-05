using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Aangeleverd.Models;

public class Product
{
    [Required(ErrorMessage = "Voer een naam in")]
    public string? artikelNaam { get; set; }
    [Required(ErrorMessage = "Voer een prijs in")]
    [Range(0, 100000.00, ErrorMessage = "Voer een prijs in tussen de 0 en 100000,00")]
    public double? artikelPrijs { get; set; }
    [BindProperty, Display(Name = "Artikel Afbeelding")]
    public IFormFile ImageFile { get; set; }
    public string? artikelAfbeelding { get; set; }
    public int artikelNummer { get; set; }

    public string? Maat { get; set; }
}