using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BarkNParkApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace BarkNParkApplication.Controllers
{
    public class MVCController : Controller
    {
        ASystem system = new ASystem(new Models.BarkParkContext());
        SystemController controller = new SystemController();
        [Route("/")]
        public IActionResult Index()
        {
            var users = system.SystemUsers;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach(Users u in users)
            {
                list.Add(new SelectListItem { Text = u.UserFirstname, Value = u.UserFirstname });
            }
            ViewBag.SystemUsers = list;
            return View();
        }
        [HttpGet]
        [Route("/UserForm")]
        public IActionResult UserForm()
        {
            return View();
        }

        [HttpGet]
        [Route("/CreateUser")]
        public IActionResult CreateUser(Users CreateUser)
        {
            string confMessage = "Confirmation Code is: " + controller.AddUSer(CreateUser).Value.ToString();
            ViewData["ConfMessage"] = confMessage;
            return RedirectToAction("Index", "MVC");
        }

        [HttpGet]
        [Route("/CreateAppointment")]
        public IActionResult CreateAppointment(string username, string Durations)
        {
            string confMessage = "Confirmation Code is: " + controller.CheckIn(username, Double.Parse(Durations)).Value.ToString();
             
            return RedirectToAction("Appointments", "MVC", new { SystemUsers = username , confirmation = confMessage});

        }

        public MultiSelectList GetItemList(string[] selectedValues)
        {
            List<SelectListItem> items = new List<SelectListItem>();
                      

            items.Add(new SelectListItem { Text = "Treat", Value = "1" });

            items.Add(new SelectListItem { Text = "Water", Value = "2", Selected = true });

            items.Add(new SelectListItem { Text = "Toy", Value = "3" });

            return new MultiSelectList(items, "Value","Text",selectedValues);
        }

        public List<SelectListItem> GetDurationList()
        {
            List<SelectListItem> durations = new List<SelectListItem>();


            durations.Add(new SelectListItem { Text = "20", Value = "20" });

            durations.Add(new SelectListItem { Text = "30", Value = "30", Selected = true });

            durations.Add(new SelectListItem { Text = "40", Value = "40" });

            durations.Add(new SelectListItem { Text = "60", Value = "60" });

            return durations;
        }

        [HttpGet]
        [Route("/Appointments")]
        public IActionResult Appointments(string SystemUsers, string confirmation = "")
        {
            var user = system.SystemUser(SystemUsers.Trim());
            var appointments = system.SystemAppointments.Where(a => a.ApptUser == user.UserId);
            ViewData["Message"] = appointments.Count() == 0 ? "This user has no appointments" : "This user has " + appointments.Count().ToString() + " appointments";
            ViewData["Username"] = SystemUsers;
            ViewData["ConfMessage"] = confirmation;
            ViewBag.Items = GetItemList(null);
            ViewBag.Durations = GetDurationList();
            return View(appointments);

        }

        [HttpGet]
        [Route("/Extend")]
        public IActionResult Extend(string username, string Durations)
        {
            string confMessage = "Confirmation Code is: " + controller.Extend(username, Double.Parse(Durations)).Value.ToString();
            return RedirectToAction("Appointments", "MVC",new {SystemUsers = username ,confirmation = confMessage});
        }

        

        [HttpPost]
        [Route("/Dispense")]
        public IActionResult Dispense(IFormCollection form)
        {
            string items = form["items"];
            string username = form["username"];
            string confMessage = "Confirmation Code is: " + controller.DispenseItem(username, items).Value.ToString();
            return RedirectToAction("Appointments", "MVC", new { SystemUsers = username, confirmation = confMessage }); ;
        }

        [HttpGet]
        [Route("/Checkout")]
        public IActionResult Checkout(string username)
        {
           string confMessage = "Confirmation Code is: " + controller.CheckOut(username).Value.ToString();
            return RedirectToAction("Appointments", "MVC", new {SystemUsers = username, confirmation = confMessage });
        }


        [HttpGet]
        [Route("/Transactions")]
        public IActionResult Transactions()
        {
            
            return View(system.SystemTransactions);
        }

    }
}