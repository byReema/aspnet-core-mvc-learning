# ASP.NET Core MVC — Learning Project

A small ASP.NET Core MVC app (`net10.0`) built step by step to practice core MVC + C# concepts.

## Concepts covered

- **MVC basics** — Controllers, Actions, Views, Models, Razor syntax, partial views (`_BookRow.cshtml`)
- **Routing** — default convention-based route (`{controller=Home}/{action=Index}/{id?}`)
- **ViewBag** vs. strongly-typed views (`@model`)
- **Lambda expressions** — `Where`, `FirstOrDefault` on `List<Book>`
- **Session / HttpContext** — a visit counter stored server-side via `HttpContext.Session`
- **View → Controller (forms & model binding)** — `[HttpGet]`/`[HttpPost]` pair on `Create`, posting a form into a `Book` object automatically
- **Connecting to SQL Server (ADO.NET)** — `SqlConnection` / `SqlCommand` / `SqlDataReader`, reading real rows from a `Books` table
- **Stored procedures** — calling `GetRecentBooks` via `CommandType.StoredProcedure` + parameters

## Running it

1. **Database** — SQL Server (Azure SQL Edge, ARM-compatible) via Docker:
   ```bash
   docker run -e "ACCEPT_EULA=1" -e "MSSQL_SA_PASSWORD=LearnSql_2026!" \
     -p 1433:1433 --name learnsql -d mcr.microsoft.com/azure-sql-edge
   ```
   Then create the database/table (see `appsettings.json` for the exact connection string used):
   ```sql
   CREATE DATABASE BooksDb;
   CREATE TABLE Books (
       Id INT PRIMARY KEY IDENTITY(1,1),
       Title NVARCHAR(100),
       Author NVARCHAR(100),
       Year INT
   );
   CREATE PROCEDURE GetRecentBooks @MinYear INT AS
   BEGIN
       SELECT Id, Title, Author, Year FROM Books WHERE Year > @MinYear
   END;
   ```

2. **App**:
   ```bash
   dotnet run
   ```
   Then visit:
   - `/Books` — hardcoded list
   - `/Books/RecentBooks` — hardcoded list, filtered with a lambda
   - `/Books/FromDb` — real rows from SQL Server
   - `/Books/RecentBooksFromDb` — real rows via the stored procedure
   - `/Books/Create` — add a book via a form (demo only, doesn't persist yet)
   - `/` — home page with a session-based visit counter

## Note on `appsettings.json`

The connection string's password (`LearnSql_2026!`) is a throwaway local Docker test credential, not a real secret — fine as committed for a learning project like this.
