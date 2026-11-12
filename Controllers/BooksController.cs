using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class BooksController : Controller
{

    private readonly IConfiguration _config; //gives access to appsettings.json

    public BooksController(IConfiguration config)
    {
        _config = config;
    }

    public IActionResult FromDb()
    {
        string connectionString = _config.GetConnectionString("BooksDb"); // get this db info
        var books = new List<Book>();

        using ( SqlConnection connection = new SqlConnection (connectionString) )
        {
            connection.Open();

            SqlCommand command = new SqlCommand("SELECT Id, Title, Author, Year FROM Books", connection);

            using (SqlDataReader reader = command.ExecuteReader()) // opens a stream of result rows from Sql Server
        {
            while (reader.Read())
            {
                var book = new Book
                {
                    Id = reader.GetInt32(0),//numbers are column positions
                    Title = reader.GetString(1),
                    Author = reader.GetString(2),
                    Year = reader.GetInt32(3)
                };
                books.Add(book);// add this row's Book to the list
            }
        }

        return View("Index", books); // don’t look for FromDb.cshtml, use Index.cshtml instead.

        }
    }

    public IActionResult RecentBooksFromDb()
{
    string connectionString = _config.GetConnectionString("BooksDb");
    var books = new List<Book>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        SqlCommand command = new SqlCommand("GetRecentBooks", connection); // procedure name rether than raw sql
        command.CommandType = CommandType.StoredProcedure; // tells SqlCommand “treat that text as a procedure name to call, not SQL to run directly
        command.Parameters.AddWithValue("@MinYear", 2010); //var 

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var book = new Book
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Author = reader.GetString(2),
                    Year = reader.GetInt32(3)
                };
                books.Add(book);
            }
        }
    }

    return View("Index", books);
}
    public IActionResult Index()
    {
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Clean Code", Author = "Robert", Year = 2008 },
            new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Thomas", Year = 1999 },
            new Book { Id = 3, Title = "C# in Depth", Author = "Jon ", Year = 2019 }
        };

        return View(books);   // send the list to the view
    }

    public IActionResult RecentBooks()
    {
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Clean Code", Author = "Robert", Year = 2008 },
            new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Thomas", Year = 1999 },
            new Book { Id = 3, Title = "C# in Depth", Author = "Jon ", Year = 2019 }
        };

        // usually it would be from db isnted of this one
        var filteredList = books.Where(b => b.Year > 2010);

        return View("Index",filteredList);
    }

    // its the default
    //write it explicitly here : it makes the pairing with [HttpPost]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Book book)
    {
        return Content($"Got: {book.Title} by {book.Author}, {book.Year}");
    }

}