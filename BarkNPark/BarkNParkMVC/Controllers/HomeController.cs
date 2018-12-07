using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BarkNParkMVC.Models;
using BarkNParkApplication.Controllers;

namespace BarkNParkMVC.Controllers
{
    public class HomeController : Controller
    {
        SystemController systemController = new SystemController();
        public IActionResult Index()
        {
            ViewData["Users"] = systemController.GetUsers();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
