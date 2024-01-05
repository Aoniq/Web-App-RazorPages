using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Aangeleverd.Models;

using System.IO;

namespace Aangeleverd.Pages
{
    public class UpdateModel : PageModel
    {
        SqliteConnection connection;

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty(SupportsGet = true)]
        public int updateArtikel { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        private IWebHostEnvironment _environment;

        public UpdateModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "fietskleding.db";
            connection = new SqliteConnection(connectionStringBuilder.ToString());

        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("gebruiker") == null || HttpContext.Session.GetString("gebruiker") != "admin")
            {
                return RedirectToPage("Login");
            }
            if (updateArtikel != 0)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Producten WHERE ID = " + updateArtikel;
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product = new Product
                    {
                        artikelNummer = reader.GetInt32(0),
                        artikelNaam = reader.GetString(1),
                        artikelPrijs = reader.GetDouble(2),
                        artikelAfbeelding = reader.GetString(3)
                    };
                }

                connection.Close();
                return Page();
            }

            return RedirectToPage("Beheer");
        }
        public IActionResult OnPost()
        {
            if (HttpContext.Session.GetString("gebruiker") == null || HttpContext.Session.GetString("gebruiker") != "admin")
            {
                return RedirectToPage("Login");
            }
            if (Upload != null)
            {
                if (Upload.ContentType == "image/gif" ||
                    Upload.ContentType == "image/jpeg" ||
                    Upload.ContentType == "image/png")
                {
                    var uploadsFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "Images");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Upload.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Upload.CopyTo(fileStream);
                    }

                    connection.Open();
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = $"UPDATE Producten SET naam = '{Product.artikelNaam}', prijs = {Product.artikelPrijs}, afbeelding = '{uniqueFileName}' WHERE ID = {Product.artikelNummer}";

                    int result = command.ExecuteNonQuery();
                    connection.Close();
                    return RedirectToPage("Beheer");
                }
                else
                {
                    ModelState.AddModelError("Upload", "Bestand is geen afbeelding");
                }
            }
            else
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"UPDATE Producten SET naam = '{Product.artikelNaam}', prijs = {Product.artikelPrijs} WHERE ID = {Product.artikelNummer}";

                int result = command.ExecuteNonQuery();
                connection.Close();
                return RedirectToPage("Beheer");
            }

            return Page();
        }

    }
}
