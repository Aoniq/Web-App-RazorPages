using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Data.Sqlite;
using Aangeleverd.Models;


//Zet in deze pagina alle code die nodig is om de tabel producten uit te lezen
//zodat de public List gevuld worden met de juiste waarden uit de database

namespace Aangeleverd.Pages;

public class ShopModel : PageModel
{
    public List<Product> shirts = new List<Product>();
    SqliteConnection connection;

    public ShopModel()
    {
        SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
        connectionStringBuilder.DataSource = "fietskleding.db";
        connection = new SqliteConnection(connectionStringBuilder.ToString());
    }

    public void OnGet()
    {
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
    }
}


