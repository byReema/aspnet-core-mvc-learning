using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.name = "Reema";
        ViewBag.course = "ASP.NET MVC";
        ViewBag.today = DateTime.Now;

        // read the counter from this visitor's session box
        int? storedCount = HttpContext.Session.GetInt32("VisitCount");

        int count;
        if (storedCount == null)
        {
            count = 1;
        }
        else
        {
            count = storedCount.Value + 1;
        }

        // overwriting the old value, without this line the count would never actually increase across requests
        // this is where we create VisitCount
        HttpContext.Session.SetInt32("VisitCount", count);
        ViewBag.visitCount = count;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
