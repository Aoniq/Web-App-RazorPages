// At the beginning of your file, include these namespaces:
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Aangeleverd.Models;

namespace Aangeleverd.Pages
{
    public class InsertModel : PageModel
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly SqliteConnection _connection;

        [BindProperty]
        public Product Product { get; set; }

        public InsertModel(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;

            // Establish database connection
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "fietskleding.db";
            _connection = new SqliteConnection(connectionStringBuilder.ToString());
        }

        public IActionResult OnPost()
        {
            if (HttpContext.Session.GetString("gebruiker") == null || HttpContext.Session.GetString("gebruiker") != "admin")
            {
                return RedirectToPage("Login");
            }
            if (ModelState.IsValid)
            {
                IFormFile uploadedFile = Product.ImageFile;
                _connection.Open();
                SqliteCommand command = _connection.CreateCommand();
                command.CommandText = $"INSERT INTO producten (naam, prijs, afbeelding) VALUES ('{Product.artikelNaam}', '{Product.artikelPrijs}', '{uploadedFile.FileName}')";

                int result = command.ExecuteNonQuery();
                _connection.Close();

                // Get the uploaded file
               

                // Check if a file was uploaded
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    // Specify the path to save the file in the wwwroot/Images folder
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images");
                    string uniqueFileName = uploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file to the specified path
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadedFile.CopyTo(fileStream);
                    }
                }
                return RedirectToPage("Beheer");
            }
            return Page();
        }
        
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("gebruiker") == null || HttpContext.Session.GetString("gebruiker") != "admin")
            {
                return RedirectToPage("Login");
            }
            return Page();
        }
    }
}
