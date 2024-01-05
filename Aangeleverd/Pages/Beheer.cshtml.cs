using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aangeleverd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

using Microsoft.AspNetCore.Http;

namespace Aangeleverd.Pages
{
	public class BeheerModel : PageModel
    {
        public List<Product> shirts = new List<Product>();
        SqliteConnection connection;

        public BeheerModel()
        {
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
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM producten";
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                Product product = new Product();

                product.artikelNummer = reader.GetInt32(0);  //id
                product.artikelNaam = reader.GetString(1);              //naam shirts
                product.artikelPrijs = reader.GetDouble(2);             //prijs
                product.artikelAfbeelding = reader.GetString(3);        //afbeelding
                shirts.Add(product);
            }
            connection.Close();
            return Page();
        }
    }
}
