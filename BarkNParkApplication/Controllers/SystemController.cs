using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BarkNParkApplication.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        ASystem system = new ASystem(new Models.BarkParkContext());

        

        // GET api/appointments
        [HttpGet]
        [Route("/api/appointments")]
        public ObjectResult GetAppointments()
        {
           
            return new ObjectResult(system.SystemAppointments);
        }

        // GET api/stations
        [HttpGet]
        [Route("/api/stations")]
        public ObjectResult GetStations()
        {
           return new ObjectResult(system.SystemStations);
        }

        // GET api/transactions
        [HttpGet]
        [Route("/api/transactions")]
        public ObjectResult GetTransactions()
        {
            return new ObjectResult(system.SystemTransactions);
        }

        // GET api/transactions
        [HttpGet]
        [Route("/api/users")]
        public ObjectResult GetUsers()
        {
            return new ObjectResult(system.SystemUsers);
        }

        // GET api/getuser/5
        [HttpGet("{name}")]
        [Route("/api/getuser/{name}")]
        public ObjectResult GetUser(string name)
        {
            return new ObjectResult(system.SystemUser(name));
        }

        //api/adduser/{first}/{last}
        [HttpPost]
        [Route("/api/adduser")]
        public ObjectResult AddUSer([FromBody] BarkNParkApplication.Models.Users newUser)
        {
            return new ObjectResult(system.AddUser(newUser));
        }

        // GET api/checkin/sally/60
        [HttpGet("{name}, {duration}")]

        [Route("/api/checkin/{name}/{duration}")]
        public ObjectResult CheckIn(string name, double duration)
        {
            
            return new ObjectResult(system.CheckIn(name, duration));
        }

        // GET api/checkout/sally
        [HttpGet]
        [Route("/api/checkout/{name}")]
        public ObjectResult CheckOut(string name)
        {
            return new ObjectResult(system.Checkout(name));
        }

        // GET api/extend/sally/5
        [HttpGet]
        [Route("/api/extend/{name}/{duration}")]
        public ObjectResult Extend(string name, double duration)
        {
            return new ObjectResult(system.requestextension(name, duration));
        }

        // GET api/dispense/sally/item1,item2,item3
        [HttpGet]
        [Route("/api/dispense/{name}/{items}")]
        public ObjectResult DispenseItem(string name, string items)
        {
            return new ObjectResult(system.addItem(name, items.Split(',')));
        }
    }
}
