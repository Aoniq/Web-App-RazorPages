using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Data.Sqlite;
using Aangeleverd.Models;

namespace Aangeleverd.Pages
{
    public class DeleteModel : PageModel
    {
        SqliteConnection connection;

        [BindProperty(SupportsGet = true)]
        public int deleteArtikel { get; set; }

        public DeleteModel()
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
            if (deleteArtikel != 0)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM producten WHERE ID = " + deleteArtikel;
                int result = command.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToPage("Beheer");
        }
    }
}
