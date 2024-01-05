using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

using Aangeleverd.Models;

namespace Aangeleverd.Pages
{
    public class BestelModel : PageModel
    {
        SqliteConnection connection;
        

        public Product product;

        [BindProperty(SupportsGet = true)]
        public int artikelnummer { get; set; }

        public string artikelNaam { get; set; }
        public double artikelPrijs { get; set; }
        public string artikelAfbeelding { get; set; }

        [BindProperty, Required(ErrorMessage = "Voer een woonplaats in"), Display(Name = "Jouw woonplaats:")]
        public string Woonplaats { get; set; }

        [BindProperty, Required(ErrorMessage = "Voer een adres in"), Display(Name = "Jouw adres:")]
        public string Adres { get; set; }

        [BindProperty, Required(ErrorMessage = "Voer een naam in"), Display(Name = "Jouw naam:"), MaxLength(10, ErrorMessage = "Naam mag niet langer zijn dan 10 tekens")]
        public string Naam { get; set; }
        [BindProperty, Required(ErrorMessage = "Selecteer een maat"), Display(Name = "Jouw maat:")]
        public string Maat { get; set; }

        public BestelModel()
        {
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "fietskleding.db";
            connection = new SqliteConnection(connectionStringBuilder.ToString());
        }

        public IActionResult OnGet()
        {
            if (artikelnummer != 0)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM producten where id = {artikelnummer}";
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    product = new Product();
                    product.artikelNummer = reader.GetInt32(0);  //id
                    product.artikelNaam = reader.GetString(1);              //naam shirts
                    product.artikelPrijs = reader.GetDouble(2);             //prijs
                    product.artikelAfbeelding = reader.GetString(3);        //afbeelding

                    artikelnummer = product.artikelNummer;
                }
                connection.Close();
                return Page();
            }
            else
            {
                return RedirectToPage("Error");
            }
        }

public IActionResult OnPost()
{
    if (ModelState.IsValid)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "INSERT INTO bestellingen (naam, adres, woonplaats, bestelling, maat) VALUES (@Naam, @Adres, @Woonplaats, @artikelnummer, @Maat)";

                Maat = Request.Form["Maat"];

                command.Parameters.AddWithValue("@Naam", Naam);
        command.Parameters.AddWithValue("@Adres", Adres);
        command.Parameters.AddWithValue("@Woonplaats", Woonplaats);
        command.Parameters.AddWithValue("@artikelnummer", artikelnummer);
        command.Parameters.AddWithValue("@Maat", Maat); // Convert enum to string

        int result = command.ExecuteNonQuery();
        connection.Close();
        return RedirectToPage("Index");
    }

    return OnGet();
}


    }
}